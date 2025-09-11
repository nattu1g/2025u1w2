using Alchemy.Inspector;
using Common.UIs.Component;
using Common.UIs.Core;
using UnityEngine;

namespace Scripts.UI.Views

{
    public class TrainingSelectView : BaseUIView
    {
        [Title("トレーニングカード")]
        [LabelText("トレーニングカード")]
        [SerializeField] private CustomButton[] _trainingCards;
        public CustomButton[] TrainingCards => _trainingCards;
    }
}
