using Common.UIs.Core;
using UnityEngine.UIElements;

namespace App.UIs.Views
{
    /// <summary>
    /// ゲーム画面のView（UI Toolkit版）
    /// </summary>
    public class GameViewUIToolkit : BaseUIToolkitView, System.IDisposable
    {
        // UI要素
        public VisualElement UiBase { get; private set; }
        public Label PointsLabel { get; private set; }
        public Label PendingPointsLabel { get; private set; }
        public Button DropNormalCoinButton { get; private set; }
        public Button DropDenseCoinButton { get; private set; }
        public Button DropCoolingCoinButton { get; private set; }
        public Button FoldButton { get; private set; }

        public override void Initialize(VisualElement root)
        {
            Root = root;

            // 要素を取得
            UiBase = root.Q<VisualElement>("game-ui-base");
            PointsLabel = root.Q<Label>("points-label");
            PendingPointsLabel = root.Q<Label>("pending-points-label");
            DropNormalCoinButton = root.Q<Button>("drop-normal-coin-button");
            DropDenseCoinButton = root.Q<Button>("drop-dense-coin-button");
            DropCoolingCoinButton = root.Q<Button>("drop-cooling-coin-button");
            FoldButton = root.Q<Button>("fold-button");
        }

        /// <summary>
        /// ポイント表示を更新
        /// </summary>
        public void SetPoints(int points)
        {
            if (PointsLabel != null)
            {
                PointsLabel.text = $"Points: {points}";
            }
        }

        /// <summary>
        /// 含み益ポイント表示を更新
        /// </summary>
        public void SetPendingPoints(int pendingPoints)
        {
            if (PendingPointsLabel != null)
            {
                PendingPointsLabel.text = $"Pending: +{pendingPoints}";
            }
        }

        /// <summary>
        /// ボタンの有効/無効を設定
        /// </summary>
        public void SetButtonsEnabled(bool enabled)
        {
            DropNormalCoinButton?.SetEnabled(enabled);
            DropDenseCoinButton?.SetEnabled(enabled);
            DropCoolingCoinButton?.SetEnabled(enabled);
            FoldButton?.SetEnabled(enabled);
        }

        /// <summary>
        /// フォールドボタンの有効/無効を設定
        /// </summary>
        public void SetFoldButtonEnabled(bool enabled)
        {
            FoldButton?.SetEnabled(enabled);
        }

        public void Dispose()
        {
            // 必要に応じてリソース解放
        }
    }
}
