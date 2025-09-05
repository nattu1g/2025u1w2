using UnityEngine;

namespace Scripts.Models
{
    public class DrawCard
    {
        public string Name;
        public float correctionPower;
        public float correctionFate;
        public int correctionStamina;

        public DrawCard(string name, int correctionValue)
        {
            Name = name;

            correctionPower = UnityEngine.Random.Range(-10, 41) / 10.0f;
            correctionFate = UnityEngine.Random.Range(-10, 41) / 10.0f;
            correctionStamina = UnityEngine.Random.Range(3, 10) * correctionValue;

        }
    }
}
