using System;
using UnityEngine;

public class CalendarEntity
{
    public int Year { get; private set; } = 1;
    public int Month { get; private set; } = 4;
    public int TrainingCountThisMonth { get; private set; } = 0;

    // public bool IsLast => (Year == 3 && Month == 9) && (TrainingCountThisMonth == 3); // 最終週かどうかを判定するプロパティ
    public bool IsLast => (Year == 1 && Month == 9) && (TrainingCountThisMonth == 3); // 最終週かどうかを判定するプロパティ
    public bool IsMatchMonth => (Month % 4 == 1); // 1,5,9 が試合月
    public bool IsMatchWeek => IsMatchMonth && TrainingCountThisMonth == 3; // 試合月の4週目かどうかを判定するプロパティ

    public event Action OnMonthChanged;
    public event Action OnTurnAdvanced;

    public void NextTraining()
    {
        TrainingCountThisMonth++;
        OnTurnAdvanced?.Invoke();

        if (TrainingCountThisMonth >= 4)
        {
            NextMonth();
        }
    }

    private void NextMonth()
    {
        TrainingCountThisMonth = 0;
        Month++;
        if (Month > 12)
        {
            Year++;
            Month = 1;
        }

        OnMonthChanged?.Invoke();
    }
}
