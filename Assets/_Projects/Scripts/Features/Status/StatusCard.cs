using Alchemy.Inspector;
using BBSim.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BBSim.Features.Status
{
    public class StatusCard : MonoBehaviour
    {
        [Title("ステータスカード")]
        [LabelText("画像")]
        [SerializeField] private Image _image;
        public Image Image => _image;
        [LabelText("名称テキスト")]
        [SerializeField] private TextMeshProUGUI _name;
        public TextMeshProUGUI Name => _name;
        [LabelText("持久力テキスト")]
        [SerializeField] private TextMeshProUGUI _stamina;
        public TextMeshProUGUI Stamina => _stamina;
        [LabelText("筋力テキスト")]
        [SerializeField] private TextMeshProUGUI _power;
        public TextMeshProUGUI Power => _power;
        [LabelText("運命力テキスト")]
        [SerializeField] private TextMeshProUGUI _fate;
        public TextMeshProUGUI Fate => _fate;

        public void SetStatus(Student student)
        {
            Name.text = student.Name;
            Stamina.text = student.Stamina.ToString();
            Power.text = student.Power.ToString();
            Fate.text = student.Fate.ToString();
        }

    }
}
