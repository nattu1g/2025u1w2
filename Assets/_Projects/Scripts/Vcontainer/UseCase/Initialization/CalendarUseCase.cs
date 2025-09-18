
using BBSim.UIs.Core;
using BBSim.Vcontainer.Entity;
using Common.Vcontainer.UseCase.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBSim.Vcontainer.UseCase
{
    public class CalendarUseCase : IInitializableUseCase
    {
        readonly UICanvas _uiCanvas;
        readonly CalendarEntity _calendarEntity;

        public int Order => 100;

        public CalendarUseCase(
            UICanvas uiCanvas,
            CalendarEntity calendarEntity
            )
        {
            _uiCanvas = uiCanvas;
            _calendarEntity = calendarEntity;
        }

        public async UniTask InitializeAsync()
        {
            Debug.Log("[CalendarUseCase][InitializeAsync] Start");
            _calendarEntity.OnTurnAdvanced += UpdateCalendarView;
            _calendarEntity.OnMonthChanged += UpdateCalendarView;
            UpdateCalendarView();
            await UniTask.CompletedTask;
        }
        public void UpdateCalendarView()
        {
            _uiCanvas.CalendarView.Year.text = _calendarEntity.Year.ToString() + " 年目";
            _uiCanvas.CalendarView.Month.text = _calendarEntity.Month.ToString() + " 月";
            _uiCanvas.CalendarView.TrainingCountThisMonth.text = (_calendarEntity.TrainingCountThisMonth + 1).ToString() + " 週目";

            if (_calendarEntity.IsMatchWeek || _calendarEntity.IsLast)
            {
                _uiCanvas.CalendarView.Year.text = "";
                _uiCanvas.CalendarView.Month.text = "";
                _uiCanvas.CalendarView.TrainingCountThisMonth.text = "試合の日！";
            }

        }
        public void NextTraining()
        {
            _calendarEntity.NextTraining();
            UpdateCalendarView();
        }

        public bool IsMatchWeek()
        {
            return _calendarEntity.IsMatchWeek;
        }
        public bool IsLast()
        {
            return _calendarEntity.IsLast;
        }

        // public WeekType GetCurrentWeekType()
        // {
        //     if (_calendarEntity.IsLast) return WeekType.Last;
        //     if (_calendarEntity.IsMatchWeek) return WeekType.Match;
        //     return WeekType.Normal;
        // }

        public void Clear()
        {
        }

        public void Dispose()
        {
            _calendarEntity.OnTurnAdvanced -= UpdateCalendarView;
            _calendarEntity.OnMonthChanged -= UpdateCalendarView;
        }
    }
}
