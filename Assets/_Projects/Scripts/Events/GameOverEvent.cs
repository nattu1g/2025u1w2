namespace App.Events
{
    /// <summary>
    /// ゲームオーバーイベント
    /// 水が水槽からこぼれた時に発行される
    /// </summary>
    public readonly struct GameOverEvent
    {
        public readonly int FinalScore;

        public GameOverEvent(int finalScore)
        {
            FinalScore = finalScore;
        }
    }
}
