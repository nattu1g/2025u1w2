using System;
using App.UIs.Core;
using App.UIs.Views;
using Common.Vcontainer.Handler;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace App.Vcontainer.Presenter
{
    /// <summary>
    /// タイトル画面のPresenter（UI Toolkit版）
    /// </summary>
    public class TitlePresenterUIToolkit : IStartable, IDisposable
    {
        private readonly UIToolkitCanvas _uiToolkitCanvas;
        private readonly UIToolkitButtonHandler _buttonHandler;
        private readonly CompositeDisposable _disposables = new();

        private TitleViewUIToolkit _titleView;

        public TitlePresenterUIToolkit(
            UIToolkitCanvas uiToolkitCanvas,
            UIToolkitButtonHandler buttonHandler)
        {
            _uiToolkitCanvas = uiToolkitCanvas;
            _buttonHandler = buttonHandler;
        }

        public void Start()
        {
            // アーキテクチャガイドラインに従い、Start()でViewを取得
            _titleView = _uiToolkitCanvas.TitleView;

            if (_titleView == null)
            {
                Debug.LogError("TitleView is null in TitlePresenterUIToolkit.Start()");
                return;
            }

            // 初期状態でタイトル画面を表示
            _uiToolkitCanvas.Show(_titleView);

            // はじめるボタン
            _buttonHandler.SetupActionButton(_titleView.StartButton, async () =>
            {
                await OnStartClickedAsync();
            });
        }

        /// <summary>
        /// はじめるボタンクリック時の処理
        /// </summary>
        private async UniTask OnStartClickedAsync()
        {
            Debug.Log("TitlePresenterUIToolkit: Start button clicked");

            // タイトル画面を非表示
            _uiToolkitCanvas.Hide(_titleView);

            await UniTask.Yield();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
