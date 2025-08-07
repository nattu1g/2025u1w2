using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.UI;
using Scripts.Setting;
using Scripts.UI;
using Scripts.Vcontainer.Entity;
using UnityEngine;
using Scripts.UI.Component;
using Scripts.UI.Core;

namespace Scripts.Vcontainer.Handler
{
    public class ButtonHandler : IHandler
    {
        readonly UICanvas _uiCanvas;
        private readonly List<CustomButton> _managedButtons = new List<CustomButton>();
        readonly AudioEntity _audioEntity;
        public ButtonHandler(
            UICanvas uiCanvas,
            AudioEntity audioEntity
            )
        {
            _uiCanvas = uiCanvas;
            _audioEntity = audioEntity;
        }

        #region ソート昇順降順変更ボタン
        public void SetStudentSortButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            // SetupActionButton(_uiCanvas.ListParentView.StudentListView.SortButton, action, onComplete);
        }
        #endregion

        #region 部活Zoom開くボタン
        public void SetClubItemZoomOpenButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            // SetupActionButton(_uiCanvas.ListParentView.ClubItemZoomView.ShowButton, async () =>
            // {
            //     _uiCanvas.Show(_uiCanvas.ListParentView.ClubItemZoomView);
            //     // Execute the provided action after showing the view
            //     await action();
            // }, onComplete);
        }
        #endregion

        #region BGM上昇ボタン
        public void SetBgmUpButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            SetupActionButton(_uiCanvas.OptionView.BgmPlusButton, action, onComplete);
        }
        #endregion
        #region BGM下降ボタン
        public void SetBgmDownButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            SetupActionButton(_uiCanvas.OptionView.BgmMinusButton, action, onComplete);
        }
        #endregion
        #region SE上昇ボタン
        public void SetSeUpButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            SetupActionButton(_uiCanvas.OptionView.SePlusButton, action, onComplete);
        }
        #endregion
        #region SE下降ボタン
        public void SetSeDownButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            SetupActionButton(_uiCanvas.OptionView.SeMinusButton, action, onComplete);
        }
        #endregion
        #region オプション開くボタン
        public void SetOptionOpenButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            SetupActionButton(_uiCanvas.OptionView.ShowButton, async () =>
            {
                _uiCanvas.Show(_uiCanvas.OptionView);
                if (action != null)
                {
                    await action();
                }
            }, onComplete);
        }
        #endregion
        #region オプション閉じるボタン
        public void SetOptionCloseButton(Func<UniTask> action, Func<UniTask> onComplete)
        {
            SetupActionButton(_uiCanvas.OptionView.HideButton, async () =>
            {
                _uiCanvas.Hide(_uiCanvas.OptionView);
                if (action != null)
                {
                    await action();
                }
            }, onComplete);
        }
        #endregion


        public void SetupActionButton(CustomButton button, Func<UniTask> action, Func<UniTask> onComplete)
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
                // ボタンを無効化
                // SetButtonEnabled(button, false);
                button.SetIntractable(false);

                try
                {
                    // アクションを実行
                    await action();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Button action failed: {e.Message}");
                    // await ShowError("エラーが発生しました");
                }
                finally
                {
                    // 処理完了後、ボタンを再度有効化
                    button.SetIntractable(true);

                    // 終了後処理を実行
                    if (onComplete != null)
                    {
                        await onComplete();
                    }
                }
            };
        }

        public void Clear()
        {
            // このHandlerでは、ClearはDisposeと同じ責務を持つことが適切です。
            // シーン遷移などでコールバックをクリアしたい場合に呼ばれることを想定します。
            Dispose();
        }

        public void Dispose()
        {
            // 管理しているすべてのボタンのコールバックを解除します。
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
