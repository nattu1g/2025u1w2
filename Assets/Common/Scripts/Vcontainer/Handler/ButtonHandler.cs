using System;
using System.Collections.Generic;
using Common.UIs.Component;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Vcontainer.Handler
{
    /// <summary>
    /// ボタンの共通処理（連打防止など）を提供する汎用ヘルパー
    /// </summary>
    public class ButtonHandler : IDisposable
    {
        private readonly List<CustomButton> _managedButtons = new List<CustomButton>();

        // このクラスは汎用的なので、特定の依存は持たない
        public ButtonHandler()
        {
        }

        /// <summary>
        /// ボタンに非同期アクションをセットアップします。クリック時にボタンを無効化し、処理完了後に再度有効化します。
        /// </summary>
        public void SetupActionButton(CustomButton button, Func<UniTask> action, Func<UniTask> onComplete = null)
        {
            if (button == null) return;

            if (!_managedButtons.Contains(button))
            {
                _managedButtons.Add(button);
            }

            // 既存のコールバックを削除
            button.onClickCallback = null;
            // 新しいコールバックを設定
            button.onClickCallback = async () =>
            {
                button.SetIntractable(false);

                try
                {
                    if (action != null)
                    {
                        await action();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Button action failed: {e.Message}");
                }
                finally
                {
                    button.SetIntractable(true);

                    if (onComplete != null)
                    {
                        await onComplete();
                    }
                }
            };
        }

        public void Dispose()
        {
            foreach (var button in _managedButtons)
            {
                if (button != null)
                {
                    button.onClickCallback = null;
                }
            }
            _managedButtons.Clear();
        }
    }
}
