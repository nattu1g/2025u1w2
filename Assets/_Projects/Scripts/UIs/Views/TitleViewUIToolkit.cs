using System;
using Common.UIs.Core;
using UnityEngine.UIElements;

namespace App.UIs.Views
{
    /// <summary>
    /// タイトル画面のView（UI Toolkit版）
    /// </summary>
    public class TitleViewUIToolkit : BaseUIToolkitView, IDisposable
    {
        // UI要素
        public Button StartButton { get; private set; }

        public override void Initialize(VisualElement root)
        {
            Root = root;
            
            // 各要素を取得
            UiBase = root.Q<VisualElement>("title-ui-base");
            UiBackground = root.Q<VisualElement>("title-ui-background");
            StartButton = root.Q<Button>("start-button");

            // nullチェック
            if (UiBase == null) UnityEngine.Debug.LogError("TitleViewUIToolkit: title-ui-base not found");
            if (UiBackground == null) UnityEngine.Debug.LogError("TitleViewUIToolkit: title-ui-background not found");
            if (StartButton == null) UnityEngine.Debug.LogError("TitleViewUIToolkit: start-button not found");
        }

        public void Dispose()
        {
            // リソース解放処理（必要に応じて）
        }
    }
}
