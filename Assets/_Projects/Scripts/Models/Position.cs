using UnityEngine;

namespace BBSim.Models
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X:F1},{Y:F1})";
    }
}
