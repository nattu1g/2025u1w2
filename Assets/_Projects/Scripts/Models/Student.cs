using System;
using BBSim.Models;
using UnityEngine;

namespace BBSim.Models
{
    public class Student
    {
        // コートの境界
        private const float COURT_X_LIMIT = 7.5f;
        private const float COURT_Y_LIMIT = 4.3f;

        public int Id;
        public string Name;
        public int Grade;

        public int Stamina;
        public int Power;
        public int Fate;
        public int Speed;
        public float ShootRange { get; private set; }
        public Position Pos { get; set; }
        public BasketballPosition BasketballPosition { get; set; }
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
            Speed = UnityEngine.Random.Range(baseMin, baseMax + 1);
            Pos = startPos;
            HasBall = false;

            // ランダムにポジションを割り当てる
            var positions = Enum.GetValues(typeof(BasketballPosition));
            BasketballPosition = (BasketballPosition)positions.GetValue(UnityEngine.Random.Range(0, positions.Length));

            // ポジションに応じてシュートレンジを設定
            switch (BasketballPosition)
            {
                case BasketballPosition.PointGuard:
                case BasketballPosition.ShootingGuard:
                case BasketballPosition.SmallForward:
                    ShootRange = 4.0f;
                    break;
                case BasketballPosition.PowerForward:
                case BasketballPosition.Center:
                    ShootRange = 2.5f;
                    break;
            }

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
            ClampPosition();
        }

        /// <summary>
        /// 選手の位置をコート内に制限する
        /// </summary>
        private void ClampPosition()
        {
            Pos.X = Mathf.Clamp(Pos.X, -COURT_X_LIMIT, COURT_X_LIMIT);
            Pos.Y = Mathf.Clamp(Pos.Y, -COURT_Y_LIMIT, COURT_Y_LIMIT);
        }
        public override string ToString()
        {
            return $"{Name}（{Grade}年） 速さ:{Speed} 持久力:{Stamina} 筋力:{Power} 運命力:{Fate} スキル:{(HasSkill ? "有" : "無")}";
        }
    }
}
