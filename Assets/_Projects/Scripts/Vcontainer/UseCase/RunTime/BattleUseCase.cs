using BBSim.UIs.Core;
using BBSim.Vcontainer.Entity;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBSim.Vcontainer.UseCase
{
    public class BattleUseCase
    {
        readonly UICanvas _uiCanvas;
        readonly DrawCardEntity _drawCardEntity;
        readonly CalendarEntity _calendarEntity;

        private const int BattleCountLimit = 5;
        private int _battleCount = 0;


        public BattleUseCase(
            UICanvas uiCanvas,
            DrawCardEntity drawCardEntity,
            CalendarEntity calendarEntity
            )
        {
            _uiCanvas = uiCanvas;
            _drawCardEntity = drawCardEntity;
            _calendarEntity = calendarEntity;
        }
        public async UniTask Battle()
        {
            Debug.Log("[BattleUseCase] Battle() called");
            // クリア処理
            Clear();

            // プレイヤー側メンバー表示
            // 相手側メンバー表示

            // バトル用画面を開く
            _uiCanvas.Show(_uiCanvas.BattleView);

            // ドローボタンの動きを設定
            var button = _uiCanvas.BattleView.DrawButton;
            button.onClickCallback = async () =>
            {
                // ボタンを無効化
                // SetButtonEnabled(button, false);
                button.SetIntractable(false);
                _battleCount++;
                _uiCanvas.BattleView.BattleCount.text = _battleCount.ToString();
                // アクションを実行
                await OnDrawButtonPressed();
                button.SetIntractable(true);
                _uiCanvas.BattleView.DrawCard.SetActive(false);
            };

            Debug.Log("[BattleUseCase] Battle() end");

            await UniTask.CompletedTask;
        }

        private async UniTask OnDrawButtonPressed()
        {
            UpdateDrawMonsterCard(); // ドローするモンスターカードの更新
            // プレイヤー側、相手側の点数を計算する。

            // Clear();
            await UniTask.WaitForSeconds(3);

            Debug.Log("[BattleUseCase] OnDrawButtonPressed() _battleCount: " + _battleCount.ToString());
            // ５回戦終了したら画面を閉じる
            if (_battleCount >= BattleCountLimit)
            {
                Clear();
                _uiCanvas.Hide(_uiCanvas.BattleView);
            }

        }

        private void UpdateDrawMonsterCard()
        {
            _uiCanvas.BattleView.DrawCard.SetActive(true);
            var monster = _drawCardEntity.MakeMonster(_calendarEntity.Year);
            _uiCanvas.BattleView.Name.text = monster.Name;
            _uiCanvas.BattleView.CorrectionPowrText.text = monster.correctionPower.ToString();
            _uiCanvas.BattleView.CorrectionFateText.text = monster.correctionFate.ToString();
            _uiCanvas.BattleView.CorrectionStaminaText.text = monster.correctionStamina.ToString();
        }

        private void Clear()
        {
            _uiCanvas.BattleView.DrawCard.SetActive(false);
            _uiCanvas.BattleView.CorrectionPowrText.text = "0";
            _uiCanvas.BattleView.CorrectionFateText.text = "0";
            _uiCanvas.BattleView.CorrectionStaminaText.text = "0";
            _uiCanvas.BattleView.DrawButton.onClickCallback = null;
            _uiCanvas.BattleView.DrawButton.SetIntractable(true);
            _uiCanvas.BattleView.PlayerScore.text = "0";
            _uiCanvas.BattleView.OpponentScore.text = "0";
            _uiCanvas.BattleView.BattleCount.text = "0";
            _battleCount = 0;
        }

        private void Dispose()
        {
        }
    }
}
