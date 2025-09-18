using System;
using System.Collections.Generic;
using BBSim.Events;
using BBSim.Models;
using BBSim.Vcontainer.UseCase;
using MessagePipe;
using UnityEngine;

namespace BBSim.Vcontainer
{
    public class MatchSimulateUseCase
    {
        private readonly TrainingSelectUseCase _trainingSelectUseCase;
        private readonly IPublisher<WeekAdvancedEvent> _weekAdvancedPublisher;
        private List<Student> teamA;
        private List<Student> teamB;
        private System.Random rng = new System.Random();
        private List<ActionLog> logs = new List<ActionLog>();
        private int totalTime = 600; // 1試合600秒
        private int elapsedTime = 0;
        public MatchSimulateUseCase(
            TrainingSelectUseCase trainingSelectUseCase,
            IPublisher<WeekAdvancedEvent> weekAdvancedPublisher
            )
        {
            _trainingSelectUseCase = trainingSelectUseCase;
            _weekAdvancedPublisher = weekAdvancedPublisher;
        }

        // public MatchSimulateUseCase(List<Student> teamA, List<Student> teamB)
        // {
        //     this.teamA = teamA;
        //     this.teamB = teamB;
        // }

        public void RunMatch()
        {
            // 開始時、ランダムにボール保持者を決定
            Student ballHolder = teamA[rng.Next(teamA.Count)];
            ballHolder.HasBall = true;
            logs.Add(new ActionLog(elapsedTime, ballHolder.Id, "StartWithBall", ballHolder.Pos));



            while (elapsedTime < totalTime)
            {
                int possessionTime = rng.Next(5, 25); // 1回の攻防は5〜24秒
                SimulatePossession(ballHolder, possessionTime);

                elapsedTime += possessionTime;

                // 攻守交代
                ballHolder.HasBall = false;
                if (teamA.Contains(ballHolder))
                    ballHolder = teamB[rng.Next(teamB.Count)];
                else
                    ballHolder = teamA[rng.Next(teamA.Count)];
                ballHolder.HasBall = true;
                logs.Add(new ActionLog(elapsedTime, ballHolder.Id, "TakeOver", ballHolder.Pos));
            }
            logs.Add(new ActionLog(elapsedTime, -1, "EndMatch", new Position(0, 0)));
            PrintLogs();
            // 試合が終了して週を進める
            _trainingSelectUseCase.AdvanceWeek();
        }

        private void SimulatePossession(Student ballHolder, int duration)
        {
            logs.Add(new ActionLog(elapsedTime, ballHolder.Id, "PossessionStart", ballHolder.Pos));


            for (int t = 0; t < duration; t++)
            {
                // ボール保持者の動き：ゴールに向かう（簡易的に座標(50,25)をゴールとする）
                Position goal = new Position(50, 25);
                ballHolder.MoveTowards(goal, speed: 1.2f);
                logs.Add(new ActionLog(elapsedTime + t, ballHolder.Id, "Move", new Position(ballHolder.Pos.X, ballHolder.Pos.Y)));

                // 味方はランダムにポジションチェンジ
                foreach (var teammate in GetTeam(ballHolder, includeSelf: false))
                {
                    teammate.MoveTowards(new Position(rng.Next(0, 50), rng.Next(0, 50)), speed: 0.5f);
                    logs.Add(new ActionLog(elapsedTime + t, teammate.Id, "Move", new Position(teammate.Pos.X, teammate.Pos.Y)));
                }

                // ディフェンスはボール保持者へ寄せる
                foreach (var opponent in GetOpponents(ballHolder))
                {
                    opponent.MoveTowards(ballHolder.Pos, speed: 0.8f);
                    logs.Add(new ActionLog(elapsedTime + t, opponent.Id, "Mark", new Position(opponent.Pos.X, opponent.Pos.Y), ballHolder.Id));
                }
            }

            // 簡易判定：ボール保持者の集中力 + 知力で得点確率を計算
            int chance = ballHolder.Power + ballHolder.Fate;
            bool scored = rng.Next(0, 100) < chance;
            logs.Add(new ActionLog(elapsedTime + duration, ballHolder.Id, scored ? "ShootSuccess" : "ShootFail", ballHolder.Pos));
        }

        private List<Student> GetTeam(Student student, bool includeSelf = true)
        {
            var team = teamA.Contains(student) ? teamA : teamB;
            if (!includeSelf) team = new List<Student>(team.FindAll(p => p != student));
            return team;
        }

        private List<Student> GetOpponents(Student student)
        {
            return teamA.Contains(student) ? teamB : teamA;
        }
        private void PrintLogs()
        {
            // Id→Name 辞書を作る
            var nameMap = new Dictionary<int, string>();
            foreach (var p in teamA) nameMap[p.Id] = p.Name;
            foreach (var p in teamB) nameMap[p.Id] = p.Name;

            foreach (var log in logs)
            {
                string actorName = nameMap.ContainsKey(log.PlayerId) ? nameMap[log.PlayerId] : $"P{log.PlayerId}";
                string targetName = (log.TargetId.HasValue && nameMap.ContainsKey(log.TargetId.Value))
                    ? nameMap[log.TargetId.Value]
                    : null;

                if (targetName != null)
                {
                    Debug.Log($"[{log.Time:F1}s] {actorName} {log.Action} at {log.Position} → Target:{targetName}");
                }
                else
                {
                    Debug.Log($"[{log.Time:F1}s] {actorName} {log.Action} at {log.Position}");
                }
            }
        }
    }
}
