using UnityEngine;
using UnityEngine.UIElements;

namespace App.UIs.Core
{
    /// <summary>
    /// UI Toolkit専用のキャンバス
    /// </summary>
    public class UIToolkitCanvas : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;

        private VisualElement _root;
        private VisualElement _battleView;
        private VisualElement _resultView;
        private VisualElement _stageSelectView;
        private VisualElement _victoryView;

        private void Awake()
        {
            if (_uiDocument == null)
            {
                _uiDocument = GetComponent<UIDocument>();
            }

            _root = _uiDocument.rootVisualElement;

            // Viewの参照を取得
            _battleView = _root.Q<VisualElement>("BattleView");
            _resultView = _root.Q<VisualElement>("ResultView");
            _stageSelectView = _root.Q<VisualElement>("StageSelectView");
            _victoryView = _root.Q<VisualElement>("VictoryView");
        }

        /// <summary>
        /// バトル画面を表示
        /// </summary>
        public void ShowBattleView()
        {
            _battleView.style.display = DisplayStyle.Flex;
            _resultView.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// 結果画面を表示
        /// </summary>
        public void ShowResultView()
        {
            _battleView.style.display = DisplayStyle.None;
            _resultView.style.display = DisplayStyle.Flex;
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
            _battleView.style.display = DisplayStyle.None;
            _resultView.style.display = DisplayStyle.None;
            _victoryView.style.display = DisplayStyle.None;
            _stageSelectView.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// 勝利画面を表示
        /// </summary>
        public void ShowVictoryView()
        {
            _battleView.style.display = DisplayStyle.None;
            _resultView.style.display = DisplayStyle.None;
            _stageSelectView.style.display = DisplayStyle.None;
            _victoryView.style.display = DisplayStyle.Flex;
        }
    }
}
