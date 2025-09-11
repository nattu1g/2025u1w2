using Alchemy.Inspector;
using Common.UIs.Core;
using TMPro;
using UnityEngine;

namespace BBSim.UIs.Views
{
    public class CalendarView : BaseUIView
    {
        [Title("カレンダー")]
        [LabelText("年")]
        [SerializeField] private TextMeshProUGUI _year;
        public TextMeshProUGUI Year => _year;
        [LabelText("月")]
        [SerializeField] private TextMeshProUGUI _month;
        public TextMeshProUGUI Month => _month;
        [LabelText("週")]
        [SerializeField] private TextMeshProUGUI _trainingCountThisMonth;
        public TextMeshProUGUI TrainingCountThisMonth => _trainingCountThisMonth;
    }
}
