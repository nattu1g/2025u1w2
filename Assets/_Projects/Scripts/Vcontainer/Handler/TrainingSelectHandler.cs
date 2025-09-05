using Scripts.Features.Status;
using Scripts.Features.Training;
using Scripts.Models;
using Scripts.UI;
using Scripts.UI.Component;
using Scripts.UI.Core;
using Scripts.Vcontainer.Entity; // Added for PlayerClubEntity
using Scripts.Vcontainer.Handler;
using Scripts.Vcontainer.UseCase;
using UnityEngine;

namespace Scripts.Vcontainer.Handler
{
    public class TrainingSelectHandler : IHandler
    {
        readonly TrainingOptionEntity _trainingOptionEntity;
        readonly UICanvas _uiCanvas;
        readonly PlayerClubEntity _playerClubEntity;
        readonly TrainingUseCase _trainingUseCase;
        readonly CalendarUseCase _calendarUseCase;
        readonly BattleUseCase _battleUseCase;

        public TrainingSelectHandler(
            TrainingOptionEntity trainingOptionEntity,
            UICanvas uiCanvas,
            PlayerClubEntity playerClubEntity,
            TrainingUseCase trainingUseCase,
            CalendarUseCase calendarUseCase,
            BattleUseCase battleUseCase
            )
        {
            _trainingOptionEntity = trainingOptionEntity;
            _uiCanvas = uiCanvas;
            _playerClubEntity = playerClubEntity;
            _trainingUseCase = trainingUseCase;
            _calendarUseCase = calendarUseCase;
            _battleUseCase = battleUseCase;
        }

        public void Initialize()
        {
            UpdateTrainingSelectsView();
            UpdatePlayerStatusView();
            _calendarUseCase.Initialize();
        }

        public void UpdateTrainingSelectsView()
        {
            // ★試合週かどうかを判定
            if (_calendarUseCase.IsLast())
            {
                Debug.Log("最終週です");
                ShowMatchCard(); // 試合カード表示処理を呼び出す
            }
            else if (_calendarUseCase.IsMatchWeek())
            {
                ShowMatchCard(); // 試合カード表示処理を呼び出す
            }
            else
            {
                ShowNormalTrainingCards(); // 通常のトレーニングカード表示処理
            }
        }

        // ★追加: 試合カードを表示するメソッド
        private void ShowMatchCard()
        {
            // まず全てのカードを非表示にする
            foreach (var card in _uiCanvas.TrainingSelectView.TrainingCards)
            {
                card.gameObject.SetActive(false);
            }

            // 1枚目のカードだけを「試合」カードとして設定・表示
            var matchCard = _uiCanvas.TrainingSelectView.TrainingCards[0];
            matchCard.gameObject.SetActive(true);

            matchCard.GetComponent<TrainingSelect>().Name.text = "試合";
            var button = matchCard.GetComponent<CustomButton>();
            button.onClickCallback = null;
            button.onClickCallback = async () =>
            {
                // TODO: 試合を実行する処理をここに書く
                Debug.Log("試合開始！");
                await _battleUseCase.Battle();
                Debug.Log("試合終了！");
                OnTrainButtonPressed(); // 週を進める
                // UpdateTrainingSelectsView();
                // UpdatePlayerStatusView();
            };
        }

        // ★リファクタリング: 元のロジックを新しいメソッドに移動
        private void ShowNormalTrainingCards()
        {
            // 全てのカードを表示状態に戻す
            foreach (var card in _uiCanvas.TrainingSelectView.TrainingCards)
            {
                card.gameObject.SetActive(true);
            }

            var selects = _trainingOptionEntity.GetRandomOptions();
            for (int i = 0; i < _uiCanvas.TrainingSelectView.TrainingCards.Length; i++)
            {
                int index = i;
                var card = _uiCanvas.TrainingSelectView.TrainingCards[i];
                card.GetComponent<TrainingSelect>().Name.text = selects[i].Name;
                var button = card.GetComponent<CustomButton>();
                button.onClickCallback = null;
                button.onClickCallback = () =>
                {
                    _trainingUseCase.ExecuteTraining(selects[index].Type);
                    OnTrainButtonPressed();
                };
            }
        }
        /// <summary>
        /// プレイヤーステータスカードの更新
        /// </summary>
        private void UpdatePlayerStatusView()
        {
            var container = _uiCanvas.PlayerStatusView.StatusCardContainer;
            var students = _playerClubEntity.Students;

            if (students.Count > container.transform.childCount)
            {
                Debug.LogWarning("The number of students exceeds the number of StatusCards.");
            }

            for (int i = 0; i < container.transform.childCount; i++)
            {
                var cardTransform = container.transform.GetChild(i);
                var statusCard = cardTransform.GetComponent<StatusCard>();

                if (statusCard != null)
                {
                    if (i < students.Count)
                    {
                        statusCard.gameObject.SetActive(true);
                        statusCard.SetStatus(students[i]);
                    }
                    else
                    {
                        statusCard.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnTrainButtonPressed()
        {
            // Debug.Log("押されたボタンの名前は " + name + " です");

            // ExecuteTraining(name);

            if (!_calendarUseCase.IsLast()) _calendarUseCase.NextTraining();
            UpdateTrainingSelectsView();
            UpdatePlayerStatusView();
        }
        public void Clear()
        {
        }

        public void Dispose()
        {
        }
    }
}

