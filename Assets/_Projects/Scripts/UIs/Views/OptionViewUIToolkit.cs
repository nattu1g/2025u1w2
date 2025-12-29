using Common.UIs.Core;
using UnityEngine.UIElements;

namespace App.UIs.Views
{
    /// <summary>
    /// UI Toolkit版のオプション画面View
    /// </summary>
    public class OptionViewUIToolkit : BaseUIToolkitView, System.IDisposable
    {
        // ボタン
        public Button BgmMinusButton { get; private set; }
        public Button BgmPlusButton { get; private set; }
        public Button SeMinusButton { get; private set; }
        public Button SePlusButton { get; private set; }

        // ラベル
        private Label _bgmValueLabel;
        private Label _seValueLabel;

        public override void Initialize(VisualElement root)
        {
            // ルート要素を設定
            Root = root;

            // カスタム名でUI要素を取得
            UiBase = root.Q<VisualElement>("option-ui-base");
            UiBackground = root.Q<VisualElement>("option-ui-background");
            ShowButton = root.Q<Button>("option-show-button");
            HideButton = root.Q<Button>("option-hide-button");

            // ボタンを取得
            BgmMinusButton = Query<Button>("bgm-minus-button");
            BgmPlusButton = Query<Button>("bgm-plus-button");
            SeMinusButton = Query<Button>("se-minus-button");
            SePlusButton = Query<Button>("se-plus-button");

            // ラベルを取得
            _bgmValueLabel = Query<Label>("bgm-value-label");
            _seValueLabel = Query<Label>("se-value-label");
        }

        /// <summary>
        /// BGM音量の表示を更新
        /// </summary>
        public void SetBgmText(float value)
        {
            if (_bgmValueLabel != null)
            {
                _bgmValueLabel.text = ((int)value).ToString();
            }
        }

        /// <summary>
        /// SE音量の表示を更新
        /// </summary>
        public void SetSeText(float value)
        {
            if (_seValueLabel != null)
            {
                _seValueLabel.text = ((int)value).ToString();
            }
        }

        public void Dispose()
        {
            // 必要に応じてリソース解放処理を追加
        }
    }
}
