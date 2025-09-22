using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BBSim;
using BBSim.UIs.Core;
using BBSim.Features;
using BBSim.Models;
using BBSim.Vcontainer.Entity;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
namespace BBSim.Vcontainer.Presenter
{
    public class MatchPresenter : IStartable
    {
        private readonly MatchSimulateUseCase _matchSimulateUseCase;
        private readonly TeamEntity _teamEntity;
        private readonly UICanvas _uiCanvas;

        private readonly MatchAssembly _matchAssembly;

        private Dictionary<int, GameObject> _playerObjects = new Dictionary<int, GameObject>();
        private GameObject _ballObject;
        private int? _currentBallHolderId;
        private bool _isBallAnimating = false;
        private bool _isPlaybackRunning = false;

        // 選手IDと選手オブジェクト、プレイヤーチームIDのリストを保持
        private Dictionary<int, Student> _allPlayers = new Dictionary<int, Student>();
        private List<int> _playerTeamIds = new List<int>();

        // [デバッグ用] マークマン情報を保持
        private Dictionary<int, int> _playerDefenseAssignments = new Dictionary<int, int>();
        private Dictionary<int, int> _opponentDefenseAssignments = new Dictionary<int, int>();

        // ポジションごとのカラーを定義
        private readonly Dictionary<BasketballPosition, Color> _positionColors = new Dictionary<BasketballPosition, Color>
        {
            { BasketballPosition.PointGuard, Color.yellow },
            { BasketballPosition.ShootingGuard, Color.green },
            { BasketballPosition.SmallForward, Color.red },
            { BasketballPosition.PowerForward, Color.black },
            { BasketballPosition.Center, Color.white }
        };


        public MatchPresenter(MatchSimulateUseCase matchSimulateUseCase, TeamEntity teamEntity, MatchAssembly matchAssembly, UICanvas uiCanvas)
        {
            _matchSimulateUseCase = matchSimulateUseCase;
            _teamEntity = teamEntity;
            _matchAssembly = matchAssembly;
            _uiCanvas = uiCanvas;
        }

        public void Start()
        {
            // Presenterが初期化された時点では何もしない
        }

        public async UniTask StartMatchPlaybackAsync()
        {
            if (_isPlaybackRunning) return; // すでに再生中なら何もしない
            _isPlaybackRunning = true;

            // GameViewを表示し、スコアをリセット
            _uiCanvas.Show(_uiCanvas.GameView);
            _uiCanvas.GameView.PlayerScore.text = "0";
            _uiCanvas.GameView.OpponentScore.text = "0";
            _uiCanvas.GameView.TimeText.text = "00:00";

            _uiCanvas.GameView.PlayerEvent.text = "";
            _uiCanvas.GameView.OpponentEvent.text = "";
            // デバッグ用UIの初期化
            _uiCanvas.GameView.PlayerMarkman.text = "";
            _uiCanvas.GameView.OpponentMarkman.text = "";

            try
            {
                // 1. 試合のイベントリストを生成
                var matchEvents = _matchSimulateUseCase.SimulateMatch();

                // 2. プレイヤーのGameObjectを生成
                SetupMatchObjects(matchEvents);

                // 3. イベントを再生 (イベントベースの再生方式に変更)
                float lastEventTime = 0f;

                // デバッグ用辞書のクリア
                _playerDefenseAssignments.Clear();
                _opponentDefenseAssignments.Clear();
                float playbackSpeed = 1.0f; // 再生速度。小さいほど遅くなる。

                var sortedEvents = matchEvents.OrderBy(e => e.Time).ToList();

                foreach (var ev in sortedEvents)
                {
                    // 前のイベントからの経過時間分だけ待機する
                    float timeSinceLastEvent = ev.Time - lastEventTime;
                    if (timeSinceLastEvent > 0.01f) // 小さすぎる待機はスキップ
                    {
                        await UniTask.Delay(System.TimeSpan.FromSeconds(timeSinceLastEvent / playbackSpeed));
                    }
                    lastEventTime = ev.Time;

                    // 時間表示を更新
                    var timeSpan = System.TimeSpan.FromSeconds(ev.Time);
                    _uiCanvas.GameView.TimeText.text = timeSpan.ToString(@"mm\:ss");

                    // イベントの種類に応じて処理
                    switch (ev)
                    {
                        case PlayerActionEvent actionEvent:
                            {
                                if (_playerObjects.TryGetValue(actionEvent.PlayerId, out var playerObj) && playerObj != null)
                                {
                                    // "Move" アクションをアニメーションで再生
                                    AnimatePlayerAsync(playerObj, new Vector3(actionEvent.Position.X, actionEvent.Position.Y, 0), 0.8f).Forget();
                                }

                                // [デバッグ用] "Mark" アクションを検知してマークマン情報を更新
                                if (actionEvent.Action == "Mark" && actionEvent.TargetId.HasValue)
                                {
                                    int defenderId = actionEvent.PlayerId;
                                    int targetId = actionEvent.TargetId.Value;

                                    if (_playerTeamIds.Contains(defenderId))
                                    {
                                        _playerDefenseAssignments[defenderId] = targetId;
                                    }
                                    else
                                    {
                                        _opponentDefenseAssignments[defenderId] = targetId;
                                    }
                                    UpdateMarkmanDebugView();
                                }

                                // イベントテキストの更新
                                if (_allPlayers.TryGetValue(actionEvent.PlayerId, out var player))
                                {
                                    string eventText = "";
                                    switch (actionEvent.Action)
                                    {
                                        case "Pass":
                                            if (actionEvent.TargetId.HasValue && _allPlayers.TryGetValue(actionEvent.TargetId.Value, out var target))
                                            {
                                                eventText = $"{player.Name}から{target.Name}へパス";
                                            }
                                            break;
                                        case "ShootSuccess":
                                            eventText = $"{player.Name}のシュート！ GOAL!";
                                            break;
                                        case "ShootFail":
                                            eventText = $"{player.Name}のシュート！ Failed";
                                            break;
                                        case "DriveSuccess":
                                            eventText = $"{player.Name}がドライブで切り込む！";
                                            break;
                                        case "DriveFail":
                                            eventText = $"{player.Name}のドライブは止められた！";
                                            break;
                                    }

                                    if (!string.IsNullOrEmpty(eventText))
                                    {
                                        // どちらのチームのイベントか判定してUIを更新
                                        bool isPlayerTeamEvent = _playerTeamIds.Contains(player.Id);
                                        _uiCanvas.GameView.PlayerEvent.text = isPlayerTeamEvent ? eventText : "";
                                        _uiCanvas.GameView.OpponentEvent.text = isPlayerTeamEvent ? "" : eventText;
                                    }
                                }

                                if (actionEvent.Action == "Pass" && actionEvent.TargetId.HasValue)
                                {
                                    // パスの開始位置は、パスを出す選手(actionEvent.PlayerId)の位置
                                    if (_playerObjects.TryGetValue(actionEvent.PlayerId, out var passer) &&
                                        _playerObjects.TryGetValue(actionEvent.TargetId.Value, out var newHolder))
                                    {
                                        // ボールをパス開始位置に瞬間移動させてからアニメーションを開始
                                        _ballObject.transform.position = passer.transform.position;

                                        _currentBallHolderId = actionEvent.TargetId.Value;
                                        AnimateBallAsync(passer.transform.position, newHolder.transform, 0.2f).Forget();
                                    }
                                }
                                else if (actionEvent.Action.Contains("Shoot"))
                                {
                                    bool isSuccess = actionEvent.Action == "ShootSuccess";
                                    var goalPosition = GetTargetGoalPosition(actionEvent.PlayerId);
                                    AnimateBallAsync(_ballObject.transform.position, goalPosition, 0.4f, isSuccess).Forget();
                                    _currentBallHolderId = null;
                                }
                            }
                            break;
                        case MatchStatusEvent statusEvent:
                            if (statusEvent.Status == "StartWithBall" || statusEvent.Status == "TakeOver" || statusEvent.Status == "ReadyToThrowIn")
                            {
                                // TakeOverイベントでもボール保持者を更新する
                                _currentBallHolderId = statusEvent.AssociatedPlayerId;
                                // ボールがアニメーション中でなければ、即座に新しい保持者の位置に移動させる
                                UpdateBallPosition();

                                // イベントテキストの更新
                                if (statusEvent.AssociatedPlayerId.HasValue && _allPlayers.TryGetValue(statusEvent.AssociatedPlayerId.Value, out var player))
                                {
                                    string eventText = "";
                                    if (statusEvent.Status == "TakeOver")
                                    {
                                        eventText = $"{player.Name}がボールを奪！";
                                    }

                                    if (!string.IsNullOrEmpty(eventText))
                                    {
                                        // どちらのチームのイベントか判定してUIを更新
                                        bool isPlayerTeamEvent = _playerTeamIds.Contains(player.Id);
                                        _uiCanvas.GameView.PlayerEvent.text = isPlayerTeamEvent ? eventText : "";
                                        _uiCanvas.GameView.OpponentEvent.text = isPlayerTeamEvent ? "" : eventText;
                                    }
                                }

                            }
                            break;
                        case ScoreUpdateEvent scoreEvent:
                            _uiCanvas.GameView.PlayerScore.text = scoreEvent.PlayerTeamScore.ToString();
                            _uiCanvas.GameView.OpponentScore.text = scoreEvent.OpponentTeamScore.ToString();
                            break;
                    }

                    UpdateBallPosition();
                }

                // 4. 試合終了処理
                // 少し待ってから非表示にする
                await UniTask.Delay(2000);
                _uiCanvas.Hide(_uiCanvas.GameView);
                Debug.Log("Match Playback Finished!");
                CleanupPlayerObjects();
                _matchSimulateUseCase.AdvanceWeekAfterMatch();
            }
            finally
            {
                // 処理が正常終了しても例外で終了しても、必ずフラグを倒す
                _isPlaybackRunning = false;
            }
        }

        private void SetupMatchObjects(List<IMatchEvent> matchEvents)
        {
            CleanupPlayerObjects();
            _playerObjects.Clear();
            _allPlayers.Clear();
            _playerTeamIds.Clear();
            // デバッグ用辞書のクリア
            _playerDefenseAssignments.Clear();
            _opponentDefenseAssignments.Clear();

            var playerTeam = _teamEntity.PlayerClub.GetStartingMembers();
            var opponentTeam = _teamEntity.OpponentClubs[0].GetStartingMembers();

            foreach (var student in playerTeam)
            {
                var obj = Object.Instantiate(_matchAssembly.PlayerPrefab, _matchAssembly.PlayersParent);
                obj.name = $"Player_{student.Name}";
                SetPlayerColorByPosition(obj, student);
                _playerObjects[student.Id] = obj;

                _allPlayers[student.Id] = student;
                _playerTeamIds.Add(student.Id);
            }

            foreach (var student in opponentTeam)
            {
                var obj = Object.Instantiate(_matchAssembly.OpponentPrefab, _matchAssembly.PlayersParent);
                obj.name = $"Opponent_{student.Name}";
                SetPlayerColorByPosition(obj, student);
                _playerObjects[student.Id] = obj;

                _allPlayers[student.Id] = student;
            }

            // ボールオブジェクトの生成
            _ballObject = Object.Instantiate(_matchAssembly.BallPrefab, _matchAssembly.PlayersParent);
            _ballObject.name = "Ball";

            // ボールの初期位置設定
            var firstEvent = matchEvents.FirstOrDefault();
            if (firstEvent is MatchStatusEvent statusEvent && statusEvent.AssociatedPlayerId.HasValue)
            {
                _currentBallHolderId = statusEvent.AssociatedPlayerId;
            }
            UpdateBallPosition();
        }

        private void SetPlayerColorByPosition(GameObject playerObject, Student student)
        {
            // SpriteRendererを取得して色を設定します。
            // プレハブの構成に合わせて、Imageなど他のコンポーネントに変更することも可能です。
            var renderer = playerObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                if (_positionColors.TryGetValue(student.BasketballPosition, out Color color))
                {
                    renderer.color = color;
                }
                // ポジションが見つからない場合はデフォルトの色（プレハブに設定されている色）のままになります
            }
        }

        /// <summary>
        /// ボールがアニメーション中でなければ、保持者の位置に追従させる
        /// </summary>
        private void UpdateBallPosition()
        {
            if (_isBallAnimating) return;

            if (_currentBallHolderId.HasValue && _playerObjects.TryGetValue(_currentBallHolderId.Value, out var holderObject))
            {
                // 保持者の少し足元に表示するなどの調整も可能
                _ballObject.transform.position = holderObject.transform.position;
            }
        }

        /// <summary>
        /// [デバッグ用] マークマンの情報をUIに表示する
        /// </summary>
        private void UpdateMarkmanDebugView()
        {
            // UI要素がなければ何もしない（安全策）
            if (_uiCanvas.GameView.PlayerMarkman == null || _uiCanvas.GameView.OpponentMarkman == null)
            {
                return;
            }

            var playerMarkmanText = new System.Text.StringBuilder("■ Player Team Defense\n");
            foreach (var assignment in _playerDefenseAssignments.OrderBy(kvp => kvp.Key))
            {
                if (_allPlayers.TryGetValue(assignment.Key, out var defender) &&
                    _allPlayers.TryGetValue(assignment.Value, out var target))
                {
                    playerMarkmanText.AppendLine($"{defender.Name} → {target.Name}");
                }
            }
            _uiCanvas.GameView.PlayerMarkman.text = playerMarkmanText.ToString();

            var opponentMarkmanText = new System.Text.StringBuilder("■ Opponent Team Defense\n");
            foreach (var assignment in _opponentDefenseAssignments.OrderBy(kvp => kvp.Key))
            {
                if (_allPlayers.TryGetValue(assignment.Key, out var defender) &&
                    _allPlayers.TryGetValue(assignment.Value, out var target))
                {
                    opponentMarkmanText.AppendLine($"{defender.Name} → {target.Name}");
                }
            }
            _uiCanvas.GameView.OpponentMarkman.text = opponentMarkmanText.ToString();
        }

        private Vector3 GetTargetGoalPosition(int playerId)
        {
            // プレイヤーIDからチームを判別
            bool isPlayerTeam = _teamEntity.PlayerClub.GetStartingMembers().Any(p => p.Id == playerId);
            var goalPos = isPlayerTeam ? MatchSimulateUseCase.OPPONENT_GOAL_POSITION : MatchSimulateUseCase.PLAYER_GOAL_POSITION;
            return new Vector3(goalPos.X, goalPos.Y, 0);
        }

        private async UniTask AnimateBallAsync(Vector3 startPos, Transform target, float duration)
        {
            _isBallAnimating = true;
            float timer = 0;
            while (timer < duration)
            {
                if (_ballObject == null || target == null) break; // オブジェクトが破棄されたらループを抜ける

                timer += Time.deltaTime;
                float ratio = Mathf.Sin((timer / duration) * Mathf.PI * 0.5f); // イーズアウト
                _ballObject.transform.position = Vector3.Lerp(startPos, target.position, ratio);
                await UniTask.Yield();
            }
            _isBallAnimating = false;
        }

        private async UniTask AnimateBallAsync(Vector3 startPos, Vector3 targetPos, float duration, bool isSuccess)
        {
            _isBallAnimating = true;
            float timer = 0;

            // シュートアニメーション
            while (timer < duration)
            {
                if (_ballObject == null) break; // オブジェクトが破棄されたらループを抜ける

                timer += Time.deltaTime;
                // 少し山なりに飛ぶような放物線アニメーション
                float ratio = timer / duration;
                float yOffset = Mathf.Sin(ratio * Mathf.PI) * 1.5f; // 1.5fはアーチの高さ
                Vector3 pos = Vector3.Lerp(startPos, targetPos, ratio);
                pos.y += yOffset;
                _ballObject.transform.position = pos;
                await UniTask.Yield();
            }

            if (isSuccess && _ballObject != null)
            {
                // シュート成功後、ボールをゴール下に落とすアニメーションを追加
                var fallStartPos = _ballObject.transform.position;
                var fallTargetPos = new Vector3(targetPos.x, targetPos.y - 1.5f, targetPos.z);
                float fallDuration = 0.3f;
                timer = 0;
                while (timer < fallDuration)
                {
                    if (_ballObject == null) break;
                    timer += Time.deltaTime;
                    _ballObject.transform.position = Vector3.Lerp(fallStartPos, fallTargetPos, timer / fallDuration);
                    await UniTask.Yield();
                }
            }

            _isBallAnimating = false;
        }

        private async UniTask AnimatePlayerAsync(GameObject playerObj, Vector3 targetPos, float duration)
        {
            float timer = 0;
            Vector3 startPos = playerObj.transform.position;

            while (timer < duration)
            {
                if (playerObj == null) break;

                timer += Time.deltaTime;
                float ratio = timer / duration;
                playerObj.transform.position = Vector3.Lerp(startPos, targetPos, ratio);
                await UniTask.Yield();
            }
        }

        private void CleanupPlayerObjects()
        {
            foreach (var obj in _playerObjects.Values)
            {
                if (obj != null) Object.Destroy(obj);
            }

            if (_ballObject != null) Object.Destroy(_ballObject);

            // デバッグ用辞書のクリア
            _playerDefenseAssignments.Clear();
            _opponentDefenseAssignments.Clear();
        }
    }
}
