using System.Collections.Generic;
using BBSim.Events;
using BBSim.Models;
using BBSim.Settings;
using BBSim.Vcontainer.Entity;
using BBSim.Vcontainer.UseCase;
using MessagePipe;
using UnityEngine;

namespace BBSim.Vcontainer.UseCase
{
    public class TrainingSelectUseCase
    {
        readonly CalendarUseCase _calendarUseCase;
        readonly PlayerClubEntity _playerClubEntity;
        readonly TrainingOptionEntity _trainingOptionEntity;
        private readonly IPublisher<WeekAdvancedEvent> _weekAdvancedPublisher;

        public TrainingSelectUseCase(
            CalendarUseCase calendarUseCase,
            PlayerClubEntity playerClubEntity,
            TrainingOptionEntity trainingOptionEntity,
            IPublisher<WeekAdvancedEvent> weekAdvancedPublisher
            )
        {
            _calendarUseCase = calendarUseCase;
            _playerClubEntity = playerClubEntity;
            _trainingOptionEntity = trainingOptionEntity;
            _weekAdvancedPublisher = weekAdvancedPublisher;
        }

        /// <summary>
        /// 現在の週が通常か試合か最終週かを取得します。
        /// </summary>
        public WeekType GetCurrentWeekType()
        {
            if (_calendarUseCase.IsLast()) return WeekType.Last;
            if (_calendarUseCase.IsMatchWeek()) return WeekType.Match;
            return WeekType.Normal;
        }

        /// <summary>
        /// プレイヤーが所属するクラブの生徒リストを取得します。
        /// </summary>
        public IReadOnlyList<Student> GetPlayerStudents()
        {
            // PlayerClubEntityが実際に保持しているのは Student のリストのはずです
            return _playerClubEntity.Students;
        }

        /// <summary>
        /// 表示するトレーニングの選択肢データを取得します。
        /// </summary>
        public List<TrainingOption> GetTrainingOptions()
        {
            return _trainingOptionEntity.GetRandomOptions();
        }

        /// <summary>
        /// ゲーム内の週を次に進めます。
        /// </summary>
        public void AdvanceWeek()
        {
            if (!_calendarUseCase.IsLast())
            {
                _calendarUseCase.NextTraining();
                _weekAdvancedPublisher.Publish(new WeekAdvancedEvent());
            }
        }
    }
}
