using BBSim.Events; // 中央のイベント定義をusing
using BBSim.Features.Status;
using BBSim.Features.Training;
using BBSim.Settings;
using BBSim.UIs.Core;
using Common.UIs.Component;
using MessagePipe;
using UnityEngine;
using BBSim.Vcontainer.UseCase;
using Common.Events;
using VContainer.Unity;
using Common.Features;

namespace BBSim.Vcontainer
{
    public class SchedulePresenter : IStartable
    {
        readonly UICanvas _uiCanvas;
        readonly TrainingUseCase _trainingUseCase;
        readonly TrainingSelectUseCase _trainingSelectUseCase;
        readonly MatchSimulateUseCase _matchSimulateUseCase;
        private readonly ComponentAssembly _componentAssembly;


        public SchedulePresenter(
            UICanvas uiCanvas,
            TrainingUseCase trainingUseCase,
            TrainingSelectUseCase trainingSelectUseCase,
            MatchSimulateUseCase matchSimulateUseCase,
            ISubscriber<GameInitializedEvent> gameInitializedSubscriber,
            ISubscriber<WeekAdvancedEvent> weekAdvancedSubscriber,
            ComponentAssembly componentAssembly
            )
        {
            _uiCanvas = uiCanvas;
            _trainingUseCase = trainingUseCase;
            _trainingSelectUseCase = trainingSelectUseCase;
            _matchSimulateUseCase = matchSimulateUseCase;
            _componentAssembly = componentAssembly;


            // ゲーム初期化完了と、週の進行イベントの両方を購読する
            gameInitializedSubscriber.Subscribe(_ => UpdateViews());
            weekAdvancedSubscriber.Subscribe(_ => UpdateViews());
        }

        /// <summary>
        /// 全てのビューを更新する親メソッド
        /// </summary>
        private void UpdateViews()
        {
            Debug.Log("[SchedulePresenter][UpdateViews] Start");
            var weekType = _trainingSelectUseCase.GetCurrentWeekType();
            if (weekType == WeekType.Match || weekType == WeekType.Last)
            {
                ShowMatchCard();
            }
            else
            {
                ShowNormalTrainingCards();
            }
            UpdatePlayerStatusView();
        }
        /// <summary>
        /// 試合カード
        /// </summary>
        private void ShowMatchCard()
        {
            Debug.Log("[SchedulePresenter][ShowMatchCard] Start");
            foreach (var card in _uiCanvas.TrainingSelectView.TrainingCards)
            {
                card.gameObject.SetActive(false);
            }

            var matchCard = _uiCanvas.TrainingSelectView.TrainingCards[0];
            matchCard.gameObject.SetActive(true);
            matchCard.GetComponent<TrainingSelect>().Name.text = "試合";

            var button = matchCard.GetComponent<CustomButton>();
            button.onClickCallback = null;
            button.onClickCallback = () =>
            {
                // UseCaseを呼び出すだけ。週の進行やUI更新は行わない。
                // UseCaseが処理の最後にWeekAdvancedEventを発行することを期待する。
                _matchSimulateUseCase.RunMatch();
            };
        }

        //// <summary>
        /// 通常のトレーニングカード
        /// </summary>
        private void ShowNormalTrainingCards()
        {
            Debug.Log("[SchedulePresenter][ShowNormalTrainingCards] Start");
            var trainingOptions = _trainingSelectUseCase.GetTrainingOptions();
            var trainingCards = _uiCanvas.TrainingSelectView.TrainingCards;

            foreach (var card in trainingCards)
            {
                card.gameObject.SetActive(true);
            }

            for (int i = 0; i < trainingCards.Length; i++)
            {
                if (i >= trainingOptions.Count)
                {
                    trainingCards[i].gameObject.SetActive(false);
                    continue;
                }
                var card = trainingCards[i];
                var option = trainingOptions[i];
                card.GetComponent<TrainingSelect>().Name.text = option.Name;

                var button = card.GetComponent<CustomButton>();
                button.onClickCallback = null;
                button.onClickCallback = () =>
                {
                    // UseCaseを呼び出すだけ。
                    _trainingUseCase.ExecuteTraining(option.Type);
                };
            }
        }

        //// <summary>
        /// プレイヤーのステータスカードの数値を更新する
        /// </summary>
        private void UpdatePlayerStatusView()
        {
            var container = _uiCanvas.PlayerStatusView.StatusCardContainer;
            var students = _trainingSelectUseCase.GetPlayerStudents();

            // if (students.Count > container.transform.childCount)
            // {
            //     Debug.LogWarning("The number of students exceeds the number of StatusCards.");
            // }

            // for (int i = 0; i < container.transform.childCount; i++)
            // {
            //     var cardTransform = container.transform.GetChild(i);
            //     var statusCard = cardTransform.GetComponent<StatusCard>();

            //     if (statusCard != null)
            //     {
            //         if (i < students.Count)
            //         {
            //             statusCard.gameObject.SetActive(true);
            //             statusCard.SetStatus(students[i]);
            //         }
            //         else
            //         {
            //             statusCard.gameObject.SetActive(false);
            //         }
            //     }
            // }
            // 1. まず、コンテナ内にある既存のカードを全て削除する
            // (パフォーマンスを気にする場合は、オブジェクトプール等の技術を使います)
            foreach (Transform child in container.transform)
            {
                _componentAssembly.DestoroyObject(child.gameObject);
            }

            // 2. 現在の生徒リストの数だけ、新しいカードを生成してデータをセットする
            foreach (var student in students)
            {
                var obj = _componentAssembly.MakeGameObject(_componentAssembly.StatusCardPrefab, container.transform);
                var statusCard = obj.GetComponent<StatusCard>();
                statusCard.SetStatus(student);
            }
        }

        public void Start()
        {
        }
    }
}
