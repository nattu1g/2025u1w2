using UnityEngine;

namespace BBSim.Models
{
    public class ActionLog
    {
        public float Time;       // 行動が起きた時刻
        public int PlayerId;     // 誰が
        public string Action;    // 何をしたか（例: "Move", "Pass", "Shoot"）
        public Position Position; // そのときの位置
        public int? TargetId;    // パスやマーク対象など

        public ActionLog(float time, int playerId, string action, Position pos, int? targetId = null)
        {
            Time = time;
            PlayerId = playerId;
            Action = action;
            Position = pos;
            TargetId = targetId;
        }

        public override string ToString()
        {
            string targetStr = TargetId.HasValue ? $" -> Target:{TargetId}" : "";
            return $"[{Time:F1}s] P{PlayerId} {Action} at {Position}{targetStr}";
        }
    }
}
