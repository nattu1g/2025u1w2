using System;
using Common.UIs.Core;
using UnityEngine.UIElements;

namespace App.UIs.Views
{
    /// <summary>
    /// ゲームオーバー画面のView（UI Toolkit版）
    /// </summary>
    public class GameOverViewUIToolkit : BaseUIToolkitView, IDisposable
    {
        // UI要素
        public Button RetryButton { get; private set; }
        private Label _finalScoreLabel;

        public override void Initialize(VisualElement root)
        {
            Root = root;

            // 各要素を取得
            UiBase = root.Q<VisualElement>("gameover-ui-base");
            UiBackground = root.Q<VisualElement>("gameover-ui-background");
            RetryButton = root.Q<Button>("retry-button");
            _finalScoreLabel = root.Q<Label>("final-score-label");

            // nullチェック
            if (UiBase == null) UnityEngine.Debug.LogError("GameOverViewUIToolkit: gameover-ui-base not found");
            if (UiBackground == null) UnityEngine.Debug.LogError("GameOverViewUIToolkit: gameover-ui-background not found");
            if (RetryButton == null) UnityEngine.Debug.LogError("GameOverViewUIToolkit: retry-button not found");
            if (_finalScoreLabel == null) UnityEngine.Debug.LogError("GameOverViewUIToolkit: final-score-label not found");
        }

        /// <summary>
        /// 最終スコアを表示
        /// </summary>
        public void SetFinalScore(int score)
        {
            if (_finalScoreLabel != null)
            {
                _finalScoreLabel.text = $"Score: {score}";
            }
        }

        public void Dispose()
        {
            // リソース解放処理（必要に応じて）
        }
    }
}
