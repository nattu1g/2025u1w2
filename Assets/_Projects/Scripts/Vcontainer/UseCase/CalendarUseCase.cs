using Scripts.UI.Core;
using UnityEngine;

namespace Scripts.Vcontainer.UseCase
{
    public class CalendarUseCase
    {
        readonly UICanvas _uiCanvas;
        readonly CalendarEntity _calendarEntity;


        public CalendarUseCase(
            UICanvas uiCanvas,
            CalendarEntity calendarEntity
            )
        {
            _uiCanvas = uiCanvas;
            _calendarEntity = calendarEntity;
        }

        public void Initialize()
        {
            _calendarEntity.OnTurnAdvanced += UpdateCalendarView;
            _calendarEntity.OnMonthChanged += UpdateCalendarView;
            UpdateCalendarView();
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
