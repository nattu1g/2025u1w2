namespace BBSim.Models
{
    /// <summary>
    /// 全ての試合イベントが実装するインターフェース
    /// </summary>
    public interface IMatchEvent
    {
        float Time { get; }
    }

    /// <summary>
    /// 選手の行動を記録するイベント
    /// </summary>
    public class PlayerActionEvent : IMatchEvent
    {
        public float Time { get; }
        public int PlayerId { get; }
        public string Action { get; }
        public Position Position { get; }
        public int? TargetId { get; }

        public PlayerActionEvent(float time, int playerId, string action, Position position, int? targetId = null)
        {
            Time = time;
            PlayerId = playerId;
            Action = action;
            Position = position;
            TargetId = targetId;
        }
    }

    /// <summary>
    /// スコアの更新を記録するイベント
    /// </summary>
    public class ScoreUpdateEvent : IMatchEvent
    {
        public float Time { get; }
        public int PlayerTeamScore { get; }
        public int OpponentTeamScore { get; }

        public ScoreUpdateEvent(float time, int playerTeamScore, int opponentTeamScore)
        {
            Time = time;
            PlayerTeamScore = playerTeamScore;
            OpponentTeamScore = opponentTeamScore;
        }
    }

    /// <summary>
    /// 試合の状態変化（開始、終了など）を記録するイベント
    /// </summary>
    public class MatchStatusEvent : IMatchEvent
    {
        public float Time { get; }
        public string Status { get; }
        public int? AssociatedPlayerId { get; } // 関連プレイヤーID (オプション)

        public MatchStatusEvent(float time, string status, int? associatedPlayerId = null)
        {
            Time = time;
            Status = status;
            AssociatedPlayerId = associatedPlayerId;
        }
    }
}
