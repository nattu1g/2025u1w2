using App.UIs.Views;
using Common.UIs.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.UIs.Core
{
    /// <summary>
    /// UI Toolkit専用のキャンバス
    /// </summary>
    public class UIToolkitCanvas : BaseUIToolkitCanvas
    {
        private VisualElement _root;
        private VisualElement _battleView;
        private VisualElement _resultView;
        private VisualElement _stageSelectView;
        private VisualElement _victoryView;

        // UI Toolkit版のView
        private OptionViewUIToolkit _optionView;
        public OptionViewUIToolkit OptionView => _optionView;

        private void Awake()
        {
            if (UiDocument == null)
            {
                Debug.LogError("UIDocument is not assigned!");
                return;
            }

            _root = UiDocument.rootVisualElement;

            // 既存のViewの参照を取得
            _battleView = _root.Q<VisualElement>("BattleView");
            _resultView = _root.Q<VisualElement>("ResultView");
            _stageSelectView = _root.Q<VisualElement>("StageSelectView");
            _victoryView = _root.Q<VisualElement>("VictoryView");

            // OptionViewUIToolkitの初期化
            _optionView = new OptionViewUIToolkit();
            _optionView.Initialize(_root);
            RegisterView(_optionView);
        }

        private void OnDestroy()
        {
            _optionView?.Dispose();
        }

        /// <summary>
        /// バトル画面を表示
        /// </summary>
        public void ShowBattleView()
        {
            if (_battleView != null)
            {
                _battleView.style.display = DisplayStyle.Flex;
            }
            if (_resultView != null)
            {
                _resultView.style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// 結果画面を表示
        /// </summary>
        public void ShowResultView()
        {
            if (_battleView != null)
            {
                _battleView.style.display = DisplayStyle.None;
            }
            if (_resultView != null)
            {
                _resultView.style.display = DisplayStyle.Flex;
            }
        }

        /// <summary>
        /// バトル画面のルート要素を取得
        /// </summary>
        public VisualElement GetBattleView() => _battleView;

        /// <summary>
        /// 結果画面のルート要素を取得
        /// </summary>
        public VisualElement GetResultView() => _resultView;

        /// <summary>
        /// ステージ選択画面のルート要素を取得
        /// </summary>
        public VisualElement GetStageSelectView() => _stageSelectView;

        /// <summary>
        /// ルート要素を取得
        /// </summary>
        public VisualElement GetRoot() => _root;

        /// <summary>
        /// ステージ選択画面を表示
        /// </summary>
        public void ShowStageSelectView()
        {
            if (_battleView != null) _battleView.style.display = DisplayStyle.None;
            if (_resultView != null) _resultView.style.display = DisplayStyle.None;
            if (_victoryView != null) _victoryView.style.display = DisplayStyle.None;
            if (_stageSelectView != null) _stageSelectView.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// 勝利画面を表示
        /// </summary>
        public void ShowVictoryView()
        {
            if (_battleView != null) _battleView.style.display = DisplayStyle.None;
            if (_resultView != null) _resultView.style.display = DisplayStyle.None;
            if (_stageSelectView != null) _stageSelectView.style.display = DisplayStyle.None;
            if (_victoryView != null) _victoryView.style.display = DisplayStyle.Flex;
        }
    }
}
