using System;
using System.Collections.Generic;
using System.Linq;
using BBSim.Events;
using BBSim.Models;
using BBSim.Vcontainer.Entity;
using BBSim.Vcontainer.UseCase;
using MessagePipe;
using UnityEngine;

namespace BBSim.Vcontainer
{
    public class MatchSimulateUseCase
    {
        private readonly TeamEntity _teamEntity;
        private readonly TrainingSelectUseCase _trainingSelectUseCase;
        // private readonly IPublisher<WeekAdvancedEvent> _weekAdvancedPublisher;
        private List<Student> playerTeam;
        private List<Student> opponentTeam;
        private Dictionary<Student, Student> playerDefenseAssignments; // Key: Playerディフェンダー, Value: マークする相手
        private Dictionary<Student, Student> opponentDefenseAssignments; // Key: Opponentディフェンダー, Value: マークする相手
        private System.Random rng = new System.Random();
        private List<IMatchEvent> matchEvents = new List<IMatchEvent>();
        private int playerTeamScore;
        private int opponentTeamScore;
        private int elapsedTime = 0;

        // コートの論理座標系におけるゴールの位置を定義
        public static readonly Position PLAYER_GOAL_POSITION = new Position(-7f, 0.14f);   // プレイヤー側が守るゴール（相手が攻めるゴール）
        public static readonly Position OPPONENT_GOAL_POSITION = new Position(7f, 0.14f); // 相手側が守るゴール（プレイヤーが攻めるゴール）

        #region Simulation Constants

        // --- Time & Duration ---
        private const int TotalMatchTimeInSeconds = 600; // 1試合の総時間（秒）
        private const int MinPossessionTime = 5; // 1ポゼッションの最短時間
        private const int MaxPossessionTime = 25; // 1ポゼッションの最長時間
        private const float DefaultTransitionDuration = 1.5f; // 攻守交代にかかる基本時間
        private const int TransitionSteps = 3; // 攻守交代時の動きのステップ数
        private const int DriveTimeCost = 1; // ドライブにかかる時間コスト

        // --- Court & Positioning ---
        private const float CourtSideLineX = 10.5f; // コートのサイドラインのX座標
        private const float ThrowInRestartAreaX = 5.0f; // スローイン時にパスを受ける選手が待機するエリアのX座標
        private const float CourtWidth = 8.0f; // コートの横幅
        private const float CourtHalfWidth = 4.0f; // コートの半分の横幅
        private const float PaintAreaWidth = 6.0f; // ペイントエリアの横幅
        private const float PaintAreaHalfWidth = 3.0f; // ペイントエリアの半分の横幅
        private const float FrontCourtStartX = 3.0f; // フロントコートの開始X座標
        private const float HalfLineDefenseTargetX = 1.0f; // 守備に戻る際のハーフライン付近の目標X座標
        private const float Zone1Depth = 2.3f; // オフェンスゾーン1の奥行き
        private const float Zone2Depth = 2.3f; // オフェンスゾーン2の奥行き
        private const float Zone3Depth = 2.4f; // オフェンスゾーン3の奥行き
        private const float Zone2StartX = Zone1Depth; // オフェンスゾーン2の開始X座標
        private const float Zone3StartX = Zone1Depth + Zone2Depth; // オフェンスゾーン3の開始X座標

        // --- Player Speeds ---
        private const float TransitionSpeed = 2.5f; // 攻守交代時の選手の移動速度
        private const float RunnerSpeedMultiplier = 1.5f; // トランジションで先行する選手の速度倍率
        private const float HandlerSpeedMultiplier = 1.0f; // トランジションでボールを運ぶ選手の速度倍率
        private const float DribbleToGoalSpeedMultiplier = 1.2f; // ゴールに向かってドリブルする際の速度倍率
        private const float DribbleEscapeSpeedMultiplier = 1.0f; // プレッシャーから逃れるドリブルの速度倍率
        private const float OffBallSpeedMultiplier = 0.9f; // オフボール選手の速度倍率
        private const float DefenderSpeedMultiplier = 0.8f; // ディフェンダーの速度倍率
        private const float BasePlayerSpeedFactor = 0.7f; // 選手の能力値に基づく速度の基本係数

        // --- AI Behavior ---
        private const int BasePassChance = 35; // パスを選択する基本確率 (%)
        private const int MaxPressurePassChanceBonus = 60; // プレッシャー下でパス確率に加算される最大ボーナス (%)
        private const float EffectiveShootDistance = 4.5f; // 有効なシュートチャンスと見なされるゴールからの距離
        private const int BaseDriveChance = 35; // ドライブを試みる基本確率 (%)
        private const int ShootChanceAfterDrive = 75; // ドライブ成功後にシュートを試みる確率 (%)
        private const float MinOpenDistanceForPass = 3.0f; // ドライブ後のパス対象となる味方との最低距離
        private const float DriveDistance = 4.5f; // ドライブで進む距離
        private const int DriveRandomnessFactor = 20; // ドライブ成功判定に加わるランダム要素の幅
        private const float PressureDistance = 2.0f; // ディフェンダーがこの距離以内に入るとプレッシャーがかかる
        private const float DefenderBlockingAngleThreshold = 0.85f; // ディフェンダーがシュートをブロックしていると見なす角度の閾値 (Dot積)
        private const float DefenderBlockingDistance = 1.0f; // ディフェンダーがシュートをブロックしていると見なす距離
        private const float OnBallDefenseTightness = 0.1f; // ボールマンに対するディフェンスの厳しさ (0に近いほどタイト)
        private const float OffBallDefenseTightness = 0.5f; // オフボールマンに対するディフェンスの厳しさ
        private const float ShootPressureDistance = 2.0f; // シュート成功率に影響を与えるディフェンスの距離
        private const int MaxShootDefenseDebuff = 30; // シュート成功率に対するディフェンスによる最大デバフ値
        private const int SuccessChanceMax = 100; // 確率計算の最大値 (100%)
        private const float AttractionWeight = 1.0f; // オフボールムーブ：理想のスポットへ向かう動きの重み
        private const float RepulsionWeight = 0.8f; // オフボールムーブ：味方から離れる動きの重み
        private const float EscapeWeight = 0.6f; // オフボールムーブ：マークマンから離れる動きの重み
        private const float HelpWeight = 0.4f; // オフボールムーブ：ヘルプに行く動きの重み
        private const float StayInFrontWeight = 0.2f; // オフボールムーブ：ボール保持者より前にいる動きの重み
        private const float RunnerSpacing = 2.5f; // トランジション時の先行選手間の距離
        private const float HandlerSpacing = 3.0f; // トランジション時のボール運び選手間の距離
        private const float OffBallSpacing = 4.0f; // ハーフコートオフェンス時の選手間の距離
        private const float MinHelpDistance = 3.0f; // ヘルプに行く動きを開始する最小距離
        private const float MaxHelpDistance = 8.0f; // ヘルプに行く動きを開始する最大距離
        private const float MaxDistanceFromBallHolder = 1.5f; // オフボール選手がボール保持者より後ろに下がれる最大距離
        private const float HelpRepulsionWeight = 0.5f; // ヘルプ時に味方から離れる動きの重み
        private const float TooCloseThresholdRatio = 1.7f; // 味方と近すぎると判断する距離の割合
        private const float SpacingAttractionWeight = 0.2f; // スペーシング時に理想スポットへ向かう重み
        private const float NormalRepulsionMultiplier = 0.5f; // 通常時のスペーシングの重み補正


        #endregion

        public MatchSimulateUseCase(
            TeamEntity teamEntity,
            TrainingSelectUseCase trainingSelectUseCase
            // IPublisher<WeekAdvancedEvent> weekAdvancedPublisher
            )
        {
            _teamEntity = teamEntity;
            _trainingSelectUseCase = trainingSelectUseCase;
            // _weekAdvancedPublisher = weekAdvancedPublisher;
        }

        // public MatchSimulateUseCase(List<Student> playerTeam, List<Student> opponentTeam)
        // {
        //     this.playerTeam = playerTeam;
        //     this.opponentTeam = opponentTeam;
        // }

        /// <summary>
        /// 試合のシミュレーションを実行し、イベントのリストを生成して返します。
        /// このメソッドはUIの更新や週の進行を行いません。
        /// </summary>
        /// <returns>試合中に発生したイベントのリスト</returns>
        public List<IMatchEvent> SimulateMatch()
        {
            playerTeamScore = 0;
            opponentTeamScore = 0;

            TeamSetting();

            // 開始時、ランダムにボール保持者を決定
            Student ballHolder = playerTeam[rng.Next(playerTeam.Count)];
            ballHolder.HasBall = true;
            matchEvents.Clear(); // イベントリストを初期化
            matchEvents.Add(new MatchStatusEvent(elapsedTime, "StartWithBall", ballHolder.Id));



            while (elapsedTime < TotalMatchTimeInSeconds)
            {
                var oldBallHolder = ballHolder; // 攻守交代前に誰がボールを持っていたか保持
                int possessionTime = rng.Next(MinPossessionTime, MaxPossessionTime); // 1回の攻防
                bool scored = SimulatePossession(oldBallHolder, possessionTime);

                elapsedTime += possessionTime;

                // 攻守交代
                oldBallHolder.HasBall = false;
                Student newBallHolder;

                if (scored)
                {
                    // --- ゴール後のスローインによるリスタート ---
                    var winningTeam = playerTeam.Contains(oldBallHolder) ? playerTeam : opponentTeam;
                    var losingTeam = playerTeam.Contains(oldBallHolder) ? opponentTeam : playerTeam;

                    // スローインする選手(Thrower)を先に決定
                    var thrower = losingTeam[rng.Next(losingTeam.Count)];

                    // 1. 得点したチームは守備位置（ハーフライン）へ戻る
                    float transitionDuration = DefaultTransitionDuration;
                    int steps = TransitionSteps;
                    for (int i = 1; i <= steps; i++)
                    {
                        float stepTime = elapsedTime + (transitionDuration * i / steps);
                        MoveToDefensiveTransition(winningTeam, thrower, stepTime);
                    }
                    elapsedTime += (int)transitionDuration;

                    // 2. 失点したチームがスローインの準備
                    bool isPlayerTeamRestarting = losingTeam == playerTeam;
                    float throwInX = isPlayerTeamRestarting ? -CourtSideLineX : CourtSideLineX;
                    float restartAreaX = isPlayerTeamRestarting ? -ThrowInRestartAreaX : ThrowInRestartAreaX;

                    // throwerの位置を設定
                    thrower.Pos = new Position(throwInX, (float)(rng.NextDouble() * CourtWidth - CourtHalfWidth));

                    // パスを受ける選手たち(Receivers)をコート内に配置
                    var receivers = new List<Student>(losingTeam);
                    receivers.Remove(thrower);
                    foreach (var player in receivers)
                    {
                        // player.Pos = new Position(restartAreaX + (float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * 8.0 - 4.0));
                        // matchEvents.Add(new PlayerActionEvent(elapsedTime, player.Id, "Move", new Position(player.Pos.X, player.Pos.Y)));
                        // ポジションに応じてパスを受ける位置を変更する
                        Position receiverPos;
                        switch (player.BasketballPosition)
                        {
                            case BasketballPosition.PointGuard:
                            case BasketballPosition.ShootingGuard:
                                // ガードはパスを受けやすいようにバックコートに位置取る
                                receiverPos = new Position(restartAreaX + (float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * PaintAreaWidth - PaintAreaHalfWidth));
                                break;
                            default: // SF, PF, C
                                // フォワードとセンターはゴールに近いフロントコートで待機
                                float goalSideX = isPlayerTeamRestarting ? -FrontCourtStartX : FrontCourtStartX;
                                receiverPos = new Position(goalSideX + (float)(rng.NextDouble() * 2 - 1), (float)(rng.NextDouble() * 8.0 - 4.0));
                                break;
                        }
                        player.Pos = receiverPos;
                        matchEvents.Add(new PlayerActionEvent(elapsedTime, player.Id, "Move", new Position(player.Pos.X, player.Pos.Y)));
                    }

                    // 3. スローインの準備完了イベントを発行
                    matchEvents.Add(new PlayerActionEvent(elapsedTime, thrower.Id, "Move", new Position(thrower.Pos.X, thrower.Pos.Y)));
                    matchEvents.Add(new MatchStatusEvent(elapsedTime, "ReadyToThrowIn", thrower.Id));
                    foreach (var player in receivers)
                    {
                        matchEvents.Add(new PlayerActionEvent(elapsedTime, player.Id, "Move", new Position(player.Pos.X, player.Pos.Y)));
                    }

                    // 4. スローインを実行
                    var inboundReceivers = receivers.Where(p =>
                        p.BasketballPosition == BasketballPosition.PointGuard ||
                        p.BasketballPosition == BasketballPosition.ShootingGuard).ToList();

                    newBallHolder = inboundReceivers.Any() ? inboundReceivers[rng.Next(inboundReceivers.Count)]
                                    : (receivers.Any() ? receivers[rng.Next(receivers.Count)] : thrower);
                    matchEvents.Add(new PlayerActionEvent(elapsedTime, thrower.Id, "Pass", thrower.Pos, newBallHolder.Id));

                    ballHolder = newBallHolder;
                }
                else
                {
                    // --- 通常の攻守交代（シュート失敗時） ---
                    // 最後にボールを持っていた選手のマークマンがボールを奪う
                    if (playerTeam.Contains(oldBallHolder))
                    {
                        // oldBallHolderはプレイヤーチーム。彼をマークしていた相手ディフェンダーを探す
                        newBallHolder = opponentDefenseAssignments.FirstOrDefault(kvp => kvp.Value == oldBallHolder).Key;
                    }
                    else
                    {
                        // oldBallHolderは相手チーム。彼をマークしていたプレイヤーディフェンダーを探す
                        newBallHolder = playerDefenseAssignments.FirstOrDefault(kvp => kvp.Value == oldBallHolder).Key;
                    }

                    // もしマークマンが見つからなかった場合（通常はありえない）、最も近い選手に渡す（フォールバック）
                    if (newBallHolder == null)
                    {
                        var gainingTeam = playerTeam.Contains(oldBallHolder) ? opponentTeam : playerTeam;
                        newBallHolder = gainingTeam.OrderBy(opponent => Vector2.Distance(new Vector2(oldBallHolder.Pos.X, oldBallHolder.Pos.Y), new Vector2(opponent.Pos.X, opponent.Pos.Y))).FirstOrDefault() ?? gainingTeam[rng.Next(gainingTeam.Count)];
                    }

                    // ★修正点: TakeOverイベントを、時間が進む前に発行する
                    ballHolder = newBallHolder;
                    matchEvents.Add(new MatchStatusEvent(elapsedTime, "TakeOver", ballHolder.Id));

                    // シュートを外したチームは守備に戻る (1.5秒かける)
                    var defendingTeam = playerTeam.Contains(oldBallHolder) ? playerTeam : opponentTeam;
                    float transitionDuration = DefaultTransitionDuration;
                    int steps = TransitionSteps; // 0.5秒刻み
                    for (int i = 1; i <= steps; i++)
                    {
                        float stepTime = elapsedTime + (transitionDuration * i / steps);
                        // 守備側は、新しいボール保持者（リバウンドを取った選手）をマークしつつ自陣に戻る
                        MoveToDefensiveTransition(defendingTeam, ballHolder, stepTime);
                    }
                    elapsedTime += (int)transitionDuration;
                }

                ballHolder.HasBall = true;
            }
            matchEvents.Add(new MatchStatusEvent(elapsedTime, "EndMatch"));
            PrintEvents();
            return matchEvents;
        }

        /// <summary>
        /// 攻守交代時に、守備側チームが自陣に戻る動きのイベントを生成します。
        /// </summary>
        /// <param name="defendingTeam">守備に切り替わるチーム</param>
        /// <param name="newBallHolder">次のボール保持者</param>
        /// <param name="eventTime">イベントのタイムスタンプ</param>
        private void MoveToDefensiveTransition(List<Student> defendingTeam, Student newBallHolder, float eventTime)
        {
            bool isPlayerTeamDefending = defendingTeam.Any(p => playerTeam.Contains(p));
            // 守備に戻る際の目標地点を、自陣のゴールではなくハーフライン少し手前に設定する
            float rallyPointX = isPlayerTeamDefending ? -2.0f : 2.0f;

            foreach (var defender in defendingTeam)
            {
                // 各ディフェンダーの現在のY座標を維持した、ハーフライン付近の目標地点
                var defensiveRallyPoint = new Position(rallyPointX, defender.Pos.Y);

                // 1. 次のボール保持者に向かうベクトル
                var toBallHolder = new Vector2(newBallHolder.Pos.X - defender.Pos.X, newBallHolder.Pos.Y - defender.Pos.Y).normalized;
                // 2. 自陣の目標地点に戻るベクトル
                var toRallyPoint = new Vector2(defensiveRallyPoint.X - defender.Pos.X, defensiveRallyPoint.Y - defender.Pos.Y).normalized;
                // 3. 2つのベクトルを合成して移動方向を決定（目標地点に戻る動きを優先）
                var finalMovement = (toBallHolder * 0.3f + toRallyPoint * 0.7f).normalized;

                // トランジション時は通常より速く移動する
                defender.MoveTowards(new Position(defender.Pos.X + finalMovement.x, defender.Pos.Y + finalMovement.y), speed: TransitionSpeed);
                matchEvents.Add(new PlayerActionEvent(eventTime, defender.Id, "Move", new Position(defender.Pos.X, defender.Pos.Y)));
            }
        }

        /// <summary>
        /// シュート成功後、守備側チームがハーフラインまで戻る動きのイベントを生成します。
        /// </summary>
        private void MoveToHalfLineDefense(List<Student> defendingTeam, float eventTime)
        {
            bool isPlayerTeamDefending = defendingTeam.Any(p => playerTeam.Contains(p));
            // 守るべき方向（自陣側）のハーフライン少し手前を目標にする
            float targetX = isPlayerTeamDefending ? -HalfLineDefenseTargetX : HalfLineDefenseTargetX;

            foreach (var defender in defendingTeam)
            {
                // Y座標は現在の位置を維持しつつ、X座標だけ目標位置に向かう
                var targetPos = new Position(targetX, defender.Pos.Y);

                // トランジション時は通常より速く移動する
                defender.MoveTowards(targetPos, speed: TransitionSpeed);
                matchEvents.Add(new PlayerActionEvent(eventTime, defender.Id, "Move", new Position(defender.Pos.X, defender.Pos.Y)));
            }
        }

        private void TeamSetting()
        {
            // 新しいメソッドを使ってスターティングメンバーを取得する
            playerTeam = _teamEntity.PlayerClub.GetStartingMembers();

            // 相手側はとりあえず一番手前を取得する TODO:後々、試合前に選択肢やトーナメントで変更できるようにする
            if (_teamEntity.OpponentClubs.Count > 0)
            {
                opponentTeam = _teamEntity.OpponentClubs[0].GetStartingMembers();
            }

            // マンツーマンディフェンスの割り当て
            AssignManToManDefense();
        }

        /// <summary>
        /// 各プレイヤーにマークする相手を割り当てる
        /// </summary>
        private void AssignManToManDefense()
        {
            playerDefenseAssignments = new Dictionary<Student, Student>();
            opponentDefenseAssignments = new Dictionary<Student, Student>();

            // ポジション順にソートしてからマッチアップを組むことで、マークのズレを防ぐ
            var sortedPlayerTeam = playerTeam.OrderBy(p => p.BasketballPosition).ToList();
            var sortedOpponentTeam = opponentTeam.OrderBy(p => p.BasketballPosition).ToList();

            if (sortedPlayerTeam.Count != sortedOpponentTeam.Count) return;

            for (int i = 0; i < sortedPlayerTeam.Count; i++)
            {
                // 同じポジションの選手同士をマッチアップさせる
                playerDefenseAssignments[sortedPlayerTeam[i]] = sortedOpponentTeam[i];
                opponentDefenseAssignments[sortedOpponentTeam[i]] = sortedPlayerTeam[i];
            }
        }

        private bool SimulatePossession(Student ballHolder, int duration)
        {
            matchEvents.Add(new MatchStatusEvent(elapsedTime, "PossessionStart", ballHolder.Id));
            bool canShoot = false; // シュートはハーフコートオフェンス中のみ可能

            for (int t = 0; t < duration; t++)
            {
                var attackingTeam = GetTeam(ballHolder, true);
                bool isPlayerTeamAttacking = playerTeam.Contains(ballHolder); // プレイヤーチームか判定
                float attackDirection = isPlayerTeamAttacking ? 1.0f : -1.0f; // 攻撃方向を決定

                // 攻撃側チーム全員が相手コートにいるかチェック
                bool allAttackersInHalfCourt = attackingTeam.All(p =>
                    isPlayerTeamAttacking ? p.Pos.X > 0 : p.Pos.X < 0
                );

                if (!allAttackersInHalfCourt)
                {
                    // --- トランジションフェーズ ---
                    // チーム全員で相手コートを目指す
                    canShoot = false;

                    // 役割分担：先行する選手とボールを運ぶ選手
                    var offBallAttackers = GetTeam(ballHolder, includeSelf: false);

                    // --- ポジションに基づいた役割分担 ---
                    // TODO: 将来的に、戦術に応じてこのロジックをより複雑にすることが可能

                    // ボール運びグループ(Ball Handlers)はボール保持者と、残りのガード陣
                    var ballHandlers = new List<Student> { ballHolder };
                    var otherGuards = offBallAttackers.Where(p =>
                        p.BasketballPosition == BasketballPosition.PointGuard ||
                        p.BasketballPosition == BasketballPosition.ShootingGuard).ToList();
                    ballHandlers.AddRange(otherGuards);

                    // 先行グループ(Runners)は、ボール運びグループ以外の選手
                    var runners = offBallAttackers.Except(ballHandlers).ToList();

                    // 1. 先行グループ (Runners) の動き
                    // サイドに広がりながら、速く相手コートへ向かう
                    foreach (var runner in runners)
                    {
                        // 1. 基本行動: ウィングエリアへ向かう
                        var baseMovement = new Vector2(attackDirection * 1.5f, (runner.Pos.Y > 0 ? 1 : -1) * 0.5f);

                        // 2. スペーシング: 他の攻撃選手から離れる
                        var repulsionVector = CalculateRepulsion(runner, attackingTeam, RunnerSpacing);

                        // 3. 合成して移動
                        var finalMovement = baseMovement + repulsionVector;
                        var targetPos = new Position(runner.Pos.X + finalMovement.x, runner.Pos.Y + finalMovement.y);
                        float runnerSpeed = RunnerSpeedMultiplier * (BasePlayerSpeedFactor + (runner.Speed / 100.0f));
                        runner.MoveTowards(targetPos, speed: runnerSpeed);
                        matchEvents.Add(new PlayerActionEvent(elapsedTime + t, runner.Id, "Move", new Position(runner.Pos.X, runner.Pos.Y)));
                    }

                    // 2. ボール運びグループ (Ball Handlers) の動き
                    // 中央付近で、少し遅れてボールを運ぶ
                    foreach (var handler in ballHandlers)
                    {
                        // 1. 基本行動: コート中央を前進
                        var baseMovement = new Vector2(attackDirection * 1.0f, 0);

                        // 2. スペーシング: 他の攻撃選手から離れる
                        var repulsionVector = CalculateRepulsion(handler, attackingTeam, HandlerSpacing);

                        // 3. 合成して移動
                        var finalMovement = baseMovement + repulsionVector;
                        var targetPos = new Position(handler.Pos.X + finalMovement.x, handler.Pos.Y + finalMovement.y);
                        float handlerSpeed = HandlerSpeedMultiplier * (BasePlayerSpeedFactor + (handler.Speed / 100.0f));
                        handler.MoveTowards(targetPos, speed: handlerSpeed);
                        matchEvents.Add(new PlayerActionEvent(elapsedTime + t, handler.Id, "Move", new Position(handler.Pos.X, handler.Pos.Y)));
                    }
                }
                else
                {
                    // --- ハーフコートオフェンスフェーズ ---
                    // 全員が相手コートに揃ったので、ゴールを狙う
                    canShoot = true;

                    var targetGoal = isPlayerTeamAttacking ? OPPONENT_GOAL_POSITION : PLAYER_GOAL_POSITION;
                    bool isUnderPressure = false;
                    int passChance = BasePassChance;

                    // ボール保持者をマークしているディフェンダーを探し、プレッシャーを計算する
                    var defenseAssignments = isPlayerTeamAttacking ? opponentDefenseAssignments : playerDefenseAssignments;
                    var defender = defenseAssignments.FirstOrDefault(kvp => kvp.Value == ballHolder).Key;
                    if (defender != null)
                    {
                        float distanceToDefender = Vector2.Distance(new Vector2(ballHolder.Pos.X, ballHolder.Pos.Y), new Vector2(defender.Pos.X, defender.Pos.Y));
                        if (distanceToDefender < PressureDistance)
                        {
                            // プレッシャー下と判定
                            isUnderPressure = true;
                            // ディフェンダーが近いほどパスの確率が上がる（最大75%）
                            passChance = BasePassChance + (int)((1.0f - (distanceToDefender / PressureDistance)) * MaxPressurePassChanceBonus);
                        }
                    }

                    // --- シュートチャンス判定 ---
                    float distanceToGoal = Vector2.Distance(new Vector2(ballHolder.Pos.X, ballHolder.Pos.Y), new Vector2(targetGoal.X, targetGoal.Y));
                    // ゴールから一定距離内で、かつ選手のシュートレンジ内か？
                    if (distanceToGoal < EffectiveShootDistance && distanceToGoal <= ballHolder.ShootRange)
                    {
                        bool isDefenderBlocking = false;
                        if (defender != null)
                        {
                            // ディフェンダーがボール保持者とゴールの間にいるか判定
                            Vector2 holderToGoal = new Vector2(targetGoal.X - ballHolder.Pos.X, targetGoal.Y - ballHolder.Pos.Y);
                            Vector2 holderToDefender = new Vector2(defender.Pos.X - ballHolder.Pos.X, defender.Pos.Y - ballHolder.Pos.Y);

                            // ディフェンダーが前方にいて、かつゴールより手前にいるか
                            if (Vector2.Dot(holderToGoal.normalized, holderToDefender.normalized) > DefenderBlockingAngleThreshold && // より正面に近い
                                holderToDefender.magnitude < holderToGoal.magnitude)
                            {
                                // ディフェンダーとの距離が非常に近いか
                                if (holderToDefender.magnitude < DefenderBlockingDistance)
                                {
                                    isDefenderBlocking = true;
                                }
                            }
                        }

                        if (!isDefenderBlocking)
                        {
                            // フリーでシュートチャンス！ポゼッションを終了してシュートを打つ
                            // このメソッドの最後にあるシュートロジックをここで実行し、即座にリターンする
                            return PerformShoot(ballHolder, duration, t);
                        }
                    }

                    if (rng.Next(0, SuccessChanceMax) < passChance)
                    {
                        var teammates = GetTeam(ballHolder, includeSelf: false);
                        if (teammates.Any())
                        {
                            var oldBallHolder = ballHolder;
                            Student passReceiver; // 変数名を newBallHolder から passReceiver に変更

                            if (isUnderPressure)
                            {
                                // プレッシャー下では、ゴールから最も遠い味方(アウトサイド)にパスアウトする
                                passReceiver = teammates.OrderByDescending(p => Vector2.Distance(new Vector2(p.Pos.X, p.Pos.Y), new Vector2(targetGoal.X, targetGoal.Y))).First();
                            }
                            else
                            {
                                // 通常時は、ディフェンスから最も離れている（オープンな）味方を探してパスを出す
                                var teammateDefenseAssignments = isPlayerTeamAttacking ? opponentDefenseAssignments : playerDefenseAssignments;

                                passReceiver = teammates
                                    .Select(teammate =>
                                    {
                                        // 各チームメイトをマークしているディフェンダーを探す
                                        var defender = teammateDefenseAssignments.FirstOrDefault(kvp => kvp.Value == teammate).Key;
                                        float distanceToDefender = 100f; // ディフェンダーがいない場合は非常にオープンとみなす
                                        if (defender != null)
                                        {
                                            distanceToDefender = Vector2.Distance(new Vector2(teammate.Pos.X, teammate.Pos.Y), new Vector2(defender.Pos.X, defender.Pos.Y));
                                        }
                                        // ディフェンダーとの距離を「オープン度」として返す
                                        return new { Player = teammate, Openness = distanceToDefender };
                                    })
                                    .OrderByDescending(x => x.Openness) // 最もオープンな選手を優先
                                    .FirstOrDefault()?.Player ?? teammates[rng.Next(teammates.Count)]; // 該当者がいない場合はランダム
                            }

                            oldBallHolder.HasBall = false;
                            passReceiver.HasBall = true;
                            ballHolder = passReceiver;
                            matchEvents.Add(new PlayerActionEvent(elapsedTime + t, oldBallHolder.Id, "Pass", oldBallHolder.Pos, passReceiver.Id));
                        }
                    }
                    else // パスをしない場合
                    {
                        // --- 1on1（ドライブ）判定 ---
                        int driveChance = BaseDriveChance;
                        if (!isUnderPressure && rng.Next(0, SuccessChanceMax) < driveChance)
                        {
                            bool driveSuccess = AttemptDrive(ballHolder, defender);
                            if (driveSuccess)
                            {
                                // ドライブ成功！
                                matchEvents.Add(new PlayerActionEvent(elapsedTime + t, ballHolder.Id, "DriveSuccess", ballHolder.Pos));
                                t += DriveTimeCost; // ドライブには少し時間がかかる
                                if (t >= duration) break;

                                // --- ドライブ後のアクション判定 ---
                                // ドライブでディフェンスを振り切ったので、シュートやパスのチャンスが生まれる

                                // 1. シュート判定 (高確率)
                                float newDistanceToGoal = Vector2.Distance(new Vector2(ballHolder.Pos.X, ballHolder.Pos.Y), new Vector2(targetGoal.X, targetGoal.Y));
                                if (newDistanceToGoal <= ballHolder.ShootRange)
                                {
                                    // ドライブ後はディフェンダーを振り切っていると仮定し、高い確率でシュートを試みる
                                    if (rng.Next(0, SuccessChanceMax) < ShootChanceAfterDrive)
                                    {
                                        return PerformShoot(ballHolder, duration, t);
                                    }
                                }

                                // 2. パス判定 (シュートしなかった場合)
                                var openTeammate = FindMostOpenTeammate(ballHolder, isPlayerTeamAttacking);
                                if (openTeammate != null)
                                {
                                    // オープンな味方がいればパス
                                    var oldBallHolder = ballHolder;
                                    oldBallHolder.HasBall = false;
                                    openTeammate.HasBall = true;
                                    ballHolder = openTeammate;
                                    matchEvents.Add(new PlayerActionEvent(elapsedTime + t, oldBallHolder.Id, "Pass", oldBallHolder.Pos, openTeammate.Id));
                                }
                                else
                                {
                                    // パス相手がいない場合は自分でさらにドリブル
                                    MoveBallHolder(ballHolder, false, targetGoal, t); // ドライブ後はプレッシャーがないと仮定
                                }
                                continue; // この時間ステップでのアクションは完了
                            }
                            else
                            {
                                // ドライブ失敗
                                matchEvents.Add(new PlayerActionEvent(elapsedTime + t, ballHolder.Id, "DriveFail", ballHolder.Pos));
                                isUnderPressure = true; // 止められてプレッシャーが高まる
                            }
                        }

                        // ドライブをしなかった、または失敗した場合の通常の動き
                        MoveBallHolder(ballHolder, isUnderPressure, targetGoal, t);
                    }

                    // 味方はスペーシングを意識してポジションチェンジ
                    MoveOffBallPlayers(ballHolder, attackingTeam, targetGoal, isPlayerTeamAttacking, isUnderPressure, t);
                }

                // ディフェンスの動き (フェーズに関わらず実行)
                var defenders = GetOpponents(ballHolder);
                var assignments = isPlayerTeamAttacking ? opponentDefenseAssignments : playerDefenseAssignments;
                foreach (var defender in defenders)
                {
                    if (assignments.TryGetValue(defender, out Student targetToMark))
                    {
                        // 守るべきゴールを決定
                        Position defendedGoal = playerTeam.Contains(defender)
                            ? PLAYER_GOAL_POSITION
                            : OPPONENT_GOAL_POSITION;

                        // ボールマンディフェンスかオフボールディフェンスかで距離感を変える
                        float interpolationRatio;
                        if (targetToMark.HasBall)
                        {
                            // ボールマンには厳しくつく (相手とゴールの間、かなり相手寄り)
                            interpolationRatio = OnBallDefenseTightness;
                        }
                        else
                        {
                            // オフボールマンには少し距離を取り、パスカットやカバーを意識する
                            interpolationRatio = OffBallDefenseTightness;
                        }

                        var targetPos = new Position(
                            targetToMark.Pos.X + (defendedGoal.X - targetToMark.Pos.X) * interpolationRatio,
                            targetToMark.Pos.Y + (defendedGoal.Y - targetToMark.Pos.Y) * interpolationRatio
                        );

                        float defenderSpeed = DefenderSpeedMultiplier * (BasePlayerSpeedFactor + (defender.Speed / 100.0f));
                        defender.MoveTowards(targetPos, speed: defenderSpeed);
                        matchEvents.Add(new PlayerActionEvent(elapsedTime + t, defender.Id, "Mark", new Position(defender.Pos.X, defender.Pos.Y), targetToMark.Id));
                    }
                }
            }

            // ポゼッションの最後にシュート判定
            if (canShoot)
            {
                // 攻めているゴールを特定し、シュートレンジ内かチェック
                var targetGoal = playerTeam.Contains(ballHolder) ? OPPONENT_GOAL_POSITION : PLAYER_GOAL_POSITION;
                float distanceToGoal = Vector2.Distance(new Vector2(ballHolder.Pos.X, ballHolder.Pos.Y), new Vector2(targetGoal.X, targetGoal.Y));

                if (distanceToGoal <= ballHolder.ShootRange)
                    return PerformShoot(ballHolder, duration, duration);
            }

            // ポゼッションが終了し、シュートも撃てなかった場合はターンオーバーとして扱う。
            // TakeOverイベントの発行は呼び出し元のSimulateMatchメソッドに任せる。
            return false; // シュートはしていないのでfalseを返す
        }

        private void MoveBallHolder(Student ballHolder, bool isUnderPressure, Position targetGoal, int timeStep)
        {
            float attackDirection = playerTeam.Contains(ballHolder) ? 1.0f : -1.0f;

            if (isUnderPressure && ballHolder.HasBall)
            {
                // プレッシャー下でパスもできない場合、少し外にドリブルしてスペースを作る
                var escapePos = new Position(ballHolder.Pos.X - attackDirection * 2.0f, ballHolder.Pos.Y);
                float holderSpeed = DribbleEscapeSpeedMultiplier * (BasePlayerSpeedFactor + (ballHolder.Speed / 100.0f));
                ballHolder.MoveTowards(escapePos, speed: holderSpeed);
            }
            else
            {
                // 通常時はゴールに向かうが、少しY軸にも動いて揺さぶる
                var toGoalVector = new Vector2(targetGoal.X - ballHolder.Pos.X, targetGoal.Y - ballHolder.Pos.Y).normalized;
                // Y軸方向へのランダムな動きを追加
                var sidewaysVector = new Vector2(0, (float)(rng.NextDouble() * 2.0 - 1.0)).normalized * 0.4f;
                var finalDribbleVector = toGoalVector + sidewaysVector;

                var dribbleTargetPos = new Position(ballHolder.Pos.X + finalDribbleVector.x, ballHolder.Pos.Y + finalDribbleVector.y);
                float holderSpeed = DribbleToGoalSpeedMultiplier * (BasePlayerSpeedFactor + (ballHolder.Speed / 100.0f));
                ballHolder.MoveTowards(dribbleTargetPos, speed: holderSpeed);
            }
            matchEvents.Add(new PlayerActionEvent(elapsedTime + timeStep, ballHolder.Id, "Move", new Position(ballHolder.Pos.X, ballHolder.Pos.Y)));
        }

        /// <summary>
        /// ハーフコートオフェンス時のオフボールプレイヤーの動きを制御します。
        /// </summary>
        private void MoveOffBallPlayers(Student ballHolder, List<Student> attackingTeam, Position targetGoal, bool isPlayerTeamAttacking, bool isUnderPressure, int timeStep)
        {
            foreach (var teammate in GetTeam(ballHolder, includeSelf: false))
            {
                // --- 各ベクトルの計算 ---

                // 1. マークマンから離れるベクトル (Get Open)
                var defenseAssignmentsForTeammate = isPlayerTeamAttacking ? opponentDefenseAssignments : playerDefenseAssignments;
                var defenderMarkingTeammate = defenseAssignmentsForTeammate.FirstOrDefault(kvp => kvp.Value == teammate).Key;
                Vector2 escapeVector = Vector2.zero;
                if (defenderMarkingTeammate != null)
                {
                    escapeVector = new Vector2(teammate.Pos.X - defenderMarkingTeammate.Pos.X, teammate.Pos.Y - defenderMarkingTeammate.Pos.Y).normalized;
                }

                // 2. ポジション毎の理想的な場所へ向かうベクトル (Go to Spot)
                Position idealSpot = CalculateIdealSpot(teammate, targetGoal);
                var attractionVector = new Vector2(idealSpot.X - teammate.Pos.X, idealSpot.Y - teammate.Pos.Y).normalized;

                // 3. 味方から離れるベクトル (Spacing)
                var repulsionVector = CalculateRepulsion(teammate, attackingTeam, OffBallSpacing); // 少し広めに

                // 4. ボールマンが困っている時に助けに行くベクトル (Help)
                Vector2 helpVector = Vector2.zero;
                // G, SG, SFのみがヘルプに行く
                bool canHelp = teammate.BasketballPosition == BasketballPosition.PointGuard ||
                               teammate.BasketballPosition == BasketballPosition.ShootingGuard ||
                               teammate.BasketballPosition == BasketballPosition.SmallForward;

                if (canHelp && isUnderPressure) // isUnderPressureはボール保持者に対するプレッシャー
                {
                    float distanceToBallHolder = Vector2.Distance(new Vector2(teammate.Pos.X, teammate.Pos.Y), new Vector2(ballHolder.Pos.X, ballHolder.Pos.Y));
                    if (distanceToBallHolder > MinHelpDistance && distanceToBallHolder < MaxHelpDistance) // 適切な距離の選手が寄る
                    {
                        helpVector = new Vector2(ballHolder.Pos.X - teammate.Pos.X, ballHolder.Pos.Y - teammate.Pos.Y).normalized;
                    }
                }

                // 5. 状況に応じた優先順位で最終的な動きを決定する
                Vector2 finalMovement;
                float distanceToClosestTeammate = GetDistanceToClosestTeammate(teammate, attackingTeam);

                if (helpVector != Vector2.zero) // canHelp && isUnderPressure
                {
                    // 【最優先】ヘルプが必要な状況
                    // ヘルプに向かいつつ、最低限のスペーシングは意識する
                    finalMovement = (helpVector * HelpWeight) + (repulsionVector * HelpRepulsionWeight);
                }
                else if (distanceToClosestTeammate < OffBallSpacing * TooCloseThresholdRatio)
                {
                    // 【優先】味方との距離が近すぎる状況
                    // スペースを確保することを最優先する
                    finalMovement = (repulsionVector * RepulsionWeight) + (attractionVector * SpacingAttractionWeight);
                }
                else
                {
                    // 【基本行動】通常のオフボールムーブ
                    // 理想のスポットへ向かいながら、フリーになる動きとスペーシングを意識する
                    finalMovement = (attractionVector * AttractionWeight) +
                                    (escapeVector * EscapeWeight) +
                                    (repulsionVector * (RepulsionWeight * NormalRepulsionMultiplier)); // 通常時のスペーシングは少し弱めに
                }

                // 6. 原則の適用 (ボール保持者よりゴールに近い位置を保つ)
                var nextPos = new Vector2(teammate.Pos.X + finalMovement.x, teammate.Pos.Y + finalMovement.y);
                var ballHolderPos = new Vector2(ballHolder.Pos.X, ballHolder.Pos.Y);
                var goalPos = new Vector2(targetGoal.X, targetGoal.Y);

                // チームメイトのゴールからの距離が、ボール保持者のゴールからの距離より大きくならないようにする（後ろに下がりすぎない）
                if (Vector2.Distance(nextPos, goalPos) > Vector2.Distance(ballHolderPos, goalPos) + MaxDistanceFromBallHolder)
                {
                    // ゴールに向かう方向の成分を少し強める
                    var toGoalVector = (goalPos - new Vector2(teammate.Pos.X, teammate.Pos.Y)).normalized;
                    finalMovement += toGoalVector * StayInFrontWeight;
                }

                // 7. 移動実行
                var targetPos = new Position(teammate.Pos.X + finalMovement.x, teammate.Pos.Y + finalMovement.y);
                float teammateSpeed = OffBallSpeedMultiplier * (BasePlayerSpeedFactor + (teammate.Speed / 100.0f)); // 少しスピードアップ
                teammate.MoveTowards(targetPos, speed: teammateSpeed);
                matchEvents.Add(new PlayerActionEvent(elapsedTime + timeStep, teammate.Id, "Move", new Position(teammate.Pos.X, teammate.Pos.Y)));
            }
        }

        private float GetDistanceToClosestTeammate(Student player, List<Student> team)
        {
            float minDistance = float.MaxValue;
            foreach (var otherPlayer in team)
            {
                if (otherPlayer == player) continue;

                float dx = player.Pos.X - otherPlayer.Pos.X;
                float dy = player.Pos.Y - otherPlayer.Pos.Y;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);

                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            return minDistance;
        }

        /// <summary>
        /// 選手のポジションに応じて、理想的な攻撃位置を計算します。
        /// </summary>
        private Position CalculateIdealSpot(Student player, Position targetGoal)
        {
            Position idealSpot;
            float goalX = targetGoal.X;
            float direction = Mathf.Sign(goalX);
            float idealX, idealY;

            switch (player.BasketballPosition)
            {
                case BasketballPosition.PointGuard:
                    // ゾーン①: ハーフラインから2.3mのエリア
                    idealX = direction * (float)(rng.NextDouble() * Zone1Depth);
                    idealY = (float)(rng.NextDouble() * CourtWidth - CourtHalfWidth); // コートの幅全体に広がる
                    break;
                case BasketballPosition.ShootingGuard:
                case BasketballPosition.SmallForward:
                    // ゾーン②: 2.3m ~ 4.6m のエリア
                    idealX = direction * (Zone2StartX + (float)(rng.NextDouble() * Zone2Depth));
                    idealY = (float)(rng.NextDouble() * CourtWidth - CourtHalfWidth); // コートの幅全体に広がる
                    break;
                case BasketballPosition.PowerForward:
                case BasketballPosition.Center:
                default:
                    // ゾーン③: 4.6m ~ 7m のゴールに近いエリア
                    idealX = direction * (Zone3StartX + (float)(rng.NextDouble() * Zone3Depth));
                    idealY = (float)(rng.NextDouble() * PaintAreaWidth - PaintAreaHalfWidth); // ペイントエリア周辺に絞る
                    break;
            }
            idealSpot = new Position(idealX, idealY);
            return idealSpot;
        }

        /// <summary>
        /// 最もオープンな味方を探します。ドライブ後のパスなどに使用します。
        /// </summary>
        private Student FindMostOpenTeammate(Student ballHolder, bool isPlayerTeamAttacking)
        {
            var teammates = GetTeam(ballHolder, includeSelf: false);
            if (!teammates.Any()) return null;

            var teammateDefenseAssignments = isPlayerTeamAttacking ? opponentDefenseAssignments : playerDefenseAssignments;

            var openTeammates = teammates
                .Select(teammate =>
                {
                    var defender = teammateDefenseAssignments.FirstOrDefault(kvp => kvp.Value == teammate).Key;
                    float distanceToDefender = 100f;
                    if (defender != null)
                    {
                        distanceToDefender = Vector2.Distance(new Vector2(teammate.Pos.X, teammate.Pos.Y), new Vector2(defender.Pos.X, defender.Pos.Y));
                    }
                    return new { Player = teammate, Openness = distanceToDefender };
                })
                .OrderByDescending(x => x.Openness);

            // ドライブ後はディフェンスが収縮するので、ある程度オープンな選手(3m以上)がいればパス対象とする
            return openTeammates.FirstOrDefault(x => x.Openness > MinOpenDistanceForPass)?.Player;
        }

        /// <summary>
        /// 1on1（ドライブ）を試み、その成否を返します。成功した場合、攻撃者の位置も更新します。
        /// </summary>
        private bool AttemptDrive(Student attacker, Student defender)
        {
            if (defender == null) return true; // ディフェンダーがいなければ必ず成功

            // ドライブ成功判定
            // 攻撃側の能力値 (スピード、パワー) vs 守備側の能力値
            int attackerScore = attacker.Speed + attacker.Power;
            int defenderScore = defender.Speed + defender.Power; // 本来はディフェンス能力値

            // 少しランダム性を加える
            attackerScore += rng.Next(-DriveRandomnessFactor, DriveRandomnessFactor + 1);
            defenderScore += rng.Next(-DriveRandomnessFactor, DriveRandomnessFactor + 1);

            if (attackerScore > defenderScore)
            {
                // ドライブ成功！
                // 攻撃者をディフェンダーをかわしてゴール方向に移動させる
                var goalPos = playerTeam.Contains(attacker) ? OPPONENT_GOAL_POSITION : PLAYER_GOAL_POSITION;
                var toGoalVector = new Vector2(goalPos.X - attacker.Pos.X, goalPos.Y - attacker.Pos.Y).normalized;
                float driveDistance = DriveDistance;

                // 新しい位置に選手を移動
                attacker.Pos = new Position(attacker.Pos.X + toGoalVector.x * driveDistance, attacker.Pos.Y + toGoalVector.y * driveDistance);
                return true;
            }
            else
            {
                // ドライブ失敗
                return false;
            }
        }

        private bool PerformShoot(Student ballHolder, int duration, int timeInPossession)
        {
            // シュート成功率の基本値を計算
            int baseChance = ballHolder.Power + ballHolder.Fate;
            int finalChance = baseChance;

            // ボール保持者をマークしているディフェンダーを探す
            bool isPlayerTeamAttacking = playerTeam.Contains(ballHolder);
            var defenseAssignments = isPlayerTeamAttacking ? opponentDefenseAssignments : playerDefenseAssignments;
            var defender = defenseAssignments.FirstOrDefault(kvp => kvp.Value == ballHolder).Key;

            // ディフェンダーの近さに応じてデバフを計算
            if (defender != null)
            {
                float distance = Vector2.Distance(new Vector2(ballHolder.Pos.X, ballHolder.Pos.Y), new Vector2(defender.Pos.X, defender.Pos.Y));

                if (distance < ShootPressureDistance)
                {
                    // 距離が近いほどデバフが大きくなる (最大30点減点)
                    int defenseDebuff = (int)((1.0f - (distance / ShootPressureDistance)) * MaxShootDefenseDebuff);
                    finalChance -= defenseDebuff;
                }
            }

            // 最終的な成功率で判定
            bool scored = rng.Next(0, SuccessChanceMax) < finalChance;
            // イベント発生時間は、ポゼッション開始時刻(elapsedTime) + ポゼッション内での経過時間
            float eventTime = elapsedTime + timeInPossession;
            matchEvents.Add(new PlayerActionEvent(eventTime, ballHolder.Id, scored ? "ShootSuccess" : "ShootFail", ballHolder.Pos));

            if (scored)
            {
                if (playerTeam.Contains(ballHolder)) playerTeamScore++;
                else opponentTeamScore++;
                matchEvents.Add(new ScoreUpdateEvent(eventTime, playerTeamScore, opponentTeamScore));
            }
            return scored;
        }

        /// <summary>
        /// 指定された選手と他の選手との間の反発力ベクトルを計算します。
        /// </summary>
        private Vector2 CalculateRepulsion(Student player, List<Student> team, float desiredSpacing)
        {
            var repulsionVector = new Vector2(0, 0);
            foreach (var otherPlayer in team)
            {
                if (otherPlayer == player) continue;

                float dx = player.Pos.X - otherPlayer.Pos.X;
                float dy = player.Pos.Y - otherPlayer.Pos.Y;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);

                if (distance < desiredSpacing && distance > 0.01f)
                {
                    // 距離が近いほど強く反発する
                    float repulsionStrength = (1.0f - (distance / desiredSpacing));
                    repulsionVector.x += (dx / distance) * repulsionStrength;
                    repulsionVector.y += (dy / distance) * repulsionStrength;
                }
            }
            return repulsionVector;
        }
        /// <summary>
        /// 指定されたチームの選手を取得します。
        /// </summary>
        /// <param name="student"></param>
        /// <param name="includeSelf"></param>
        /// <returns></returns>
        private List<Student> GetTeam(Student student, bool includeSelf = true)
        {
            var team = playerTeam.Contains(student) ? playerTeam : opponentTeam;
            if (!includeSelf) team = new List<Student>(team.FindAll(p => p != student)); // 自身を除外
            return team;
        }

        private List<Student> GetOpponents(Student student)
        {
            return playerTeam.Contains(student) ? opponentTeam : playerTeam;
        }
        private void PrintEvents()
        {
            // Id→Name 辞書を作る
            var nameMap = new Dictionary<int, string>();
            foreach (var p in playerTeam) nameMap[p.Id] = p.Name;
            foreach (var p in opponentTeam) nameMap[p.Id] = p.Name;

            // 時間でソートしてから表示
            foreach (var ev in matchEvents.OrderBy(e => e.Time))
            {
                // C# 7.0以降の型スイッチでイベントを処理
                switch (ev)
                {
                    case PlayerActionEvent actionEvent:
                        string actorName = nameMap.ContainsKey(actionEvent.PlayerId) ? nameMap[actionEvent.PlayerId] : $"P{actionEvent.PlayerId}";
                        string targetName = (actionEvent.TargetId.HasValue && nameMap.ContainsKey(actionEvent.TargetId.Value))
                            ? nameMap[actionEvent.TargetId.Value]
                            : null;

                        if (targetName != null)
                        {
                            Debug.Log($"[{actionEvent.Time:F1}s] {actorName} {actionEvent.Action} at {actionEvent.Position} → Target:{targetName}");
                        }
                        else
                        {
                            Debug.Log($"[{actionEvent.Time:F1}s] {actorName} {actionEvent.Action} at {actionEvent.Position}");
                        }
                        break;

                    case ScoreUpdateEvent scoreEvent:
                        Debug.Log($"[{scoreEvent.Time:F1}s] ### SCORE ### Player: {scoreEvent.PlayerTeamScore} - Opponent: {scoreEvent.OpponentTeamScore}");
                        break;

                    case MatchStatusEvent statusEvent:
                        string playerName = (statusEvent.AssociatedPlayerId.HasValue && nameMap.ContainsKey(statusEvent.AssociatedPlayerId.Value))
                            ? $" ({nameMap[statusEvent.AssociatedPlayerId.Value]})"
                            : "";
                        Debug.Log($"[{statusEvent.Time:F1}s] --- {statusEvent.Status}{playerName} ---");
                        break;
                }
            }

            Debug.Log("=====================================");
            Debug.Log($"FINAL SCORE: Player {playerTeamScore} - {opponentTeamScore} Opponent");
            Debug.Log("=====================================");
        }

        /// <summary>
        /// 試合が終了した後に週を進めます。
        /// </summary>
        public void AdvanceWeekAfterMatch()
        {
            _trainingSelectUseCase.AdvanceWeek();
        }
    }
}
