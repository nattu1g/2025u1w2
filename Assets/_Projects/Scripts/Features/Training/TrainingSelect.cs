using Alchemy.Inspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Features.Training
{
    public class TrainingSelect : MonoBehaviour
    {
        [Title("トレーニングカード")]
        [LabelText("名称")]
        [SerializeField] private TextMeshProUGUI _name;
        public TextMeshProUGUI Name => _name;
        [LabelText("画像")]
        [SerializeField] private Image _image;
        public Image Image => _image;

    }
}
