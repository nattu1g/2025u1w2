using System;
using UnityEngine;

namespace BBSim.Models
{
    public class Student
    {
        public int Id;
        public string Name;
        public int Grade;

        public int Stamina;
        public int Power;
        public int Fate;
        public Position Pos { get; set; }
        public bool HasBall; // ボールを持っているか

        public bool HasSkill;

        public Student(int id, string name, int grade, int baseMin, int baseMax, Position startPos)
        {
            Id = id;
            Name = name;
            Grade = grade;

            Stamina = UnityEngine.Random.Range(baseMin, baseMax + 1);
            Power = UnityEngine.Random.Range(baseMin, baseMax + 1);
            Fate = UnityEngine.Random.Range(baseMin, baseMax + 1);
            Pos = startPos;
            HasBall = false;

            HasSkill = false;
        }

        // 簡易移動ロジック：目標位置へ少しずつ近づく
        public void MoveTowards(Position target, float speed = 1.0f)
        {
            float dx = target.X - Pos.X;
            float dy = target.Y - Pos.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            if (distance > 0.1f)
            {
                Pos.X += dx / distance * speed;
                Pos.Y += dy / distance * speed;
            }
        }
        public override string ToString()
        {
            return $"{Name}（{Grade}年） 持久力:{Stamina} 筋力:{Power} 運命力:{Fate} スキル:{(HasSkill ? "有" : "無")}";
        }
    }
}
