using System.Collections.Generic;
using Alchemy.Inspector;
using Scripts.UI.Component;
using Scripts.UI.Core;
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
