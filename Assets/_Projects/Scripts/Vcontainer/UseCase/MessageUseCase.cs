using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using MessagePipe;
using Scripts.UI;
using UnityEngine;
using VContainer.Unity;

namespace Scripts.Vcontainer.UseCase
{
    public readonly struct MessageEvent
    {
        public readonly string Text;
        public readonly bool WindowPlace;// 0:下部ウィンドウ,1:上部ウィンドウ
        public readonly bool WaitForInput;// 0:秒数待ち,1:クリック待ち

        public MessageEvent(string text, bool windowPlace, bool waitForInput)
        {
            Text = text;
            WindowPlace = windowPlace;
            WaitForInput = waitForInput;
        }
    }
    /// <summary>
    /// メッセージを表示するUseCase。どこかでMessageEventをPublishしてくれれば勝手に受け取って表示する
    /// </summary>
    public class MessageUseCase : IInitializable, IDisposable
    {
        private readonly ISubscriber<MessageEvent> _subscriber;
        private readonly UICanvas _uiCanvas;
        private IDisposable _disposable;
        private CancellationTokenSource _cancellationUp;
        private CancellationTokenSource _cancellationDown;

        public MessageUseCase(
            ISubscriber<MessageEvent> subscriber,
            UICanvas uiCanvas
            )
        {
            _subscriber = subscriber;
            _uiCanvas = uiCanvas;
        }

        public void Initialize()
        {
            _disposable = _subscriber.Subscribe(OnMessageReceived);
        }

        private async void OnMessageReceived(MessageEvent e)
        {
            CancellationTokenSource cancellation;
            TMPro.TextMeshProUGUI textMesh;
            GameObject window;

            if (e.WindowPlace)
            {
                _cancellationDown?.Cancel();
                _cancellationDown = new CancellationTokenSource();
                cancellation = _cancellationDown;
                window = _uiCanvas.MessageView.MessageWindowDown;
                textMesh = _uiCanvas.MessageView.MessageTextDown;
            }
            else
            {
                _cancellationUp?.Cancel();
                _cancellationUp = new CancellationTokenSource();
                cancellation = _cancellationUp;
                window = _uiCanvas.MessageView.MessageWindowUp;
                textMesh = _uiCanvas.MessageView.MessageTextUp;
            }
            var token = cancellation.Token;

            try
            {
                _uiCanvas.Show(_uiCanvas.MessageView);

                window.SetActive(true);
                textMesh.maxVisibleCharacters = 0;
                textMesh.text = e.Text;

                await LMotion.Create(0, e.Text.Length, 1f)
                        .Bind(val =>
                        {
                            if (textMesh != null) textMesh.maxVisibleCharacters = val;
                        })
                        .ToUniTask(cancellationToken: token);

                // TODO:フラグによって、ページ送り待ちか、秒数でウィンドウ消すかを決める
                if (e.WaitForInput)
                {
                    // クリック待ち（ここではキー入力で代用）
                    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Z), cancellationToken: token);
                }
                else
                {
                    await UniTask.WaitForSeconds(1f, cancellationToken: token);
                }

                window.SetActive(false);
                if (!_uiCanvas.MessageView.MessageWindowUp.activeSelf && !_uiCanvas.MessageView.MessageWindowDown.activeSelf) _uiCanvas.Hide(_uiCanvas.MessageView);
            }
            catch (OperationCanceledException)
            {
                // This is expected when a new message arrives, so we ignore it.
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _cancellationUp?.Cancel();
            _cancellationUp?.Dispose();
            _cancellationDown?.Cancel();
            _cancellationDown?.Dispose();
        }
    }
}