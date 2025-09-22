using System;
using System.Collections.Generic;
using System.Linq;
using BBSim.Models;
using Cysharp.Threading.Tasks;

namespace BBSim.Vcontainer.Entity
{
    // abstract(抽象)クラスとして定義
    public abstract class ClubEntity
    {
        protected readonly StudentEntity _studentEntity;
        protected readonly List<Student> students = new();

        public string Name { get; set; }
        public IReadOnlyList<Student> Students => students;

        // コンストラクタで依存性を注入
        protected ClubEntity(StudentEntity studentEntity)
        {
            _studentEntity = studentEntity;
        }

        // virtual(仮想)メソッドとして定義。必要なら派生クラスで上書き(override)できる
        public virtual async UniTask GenerateStudent(int numberOfStudentsPerGrade)
        {
            for (int grade = 1; grade <= 3; grade++)
            {
                for (int i = 0; i < numberOfStudentsPerGrade; i++)
                {
                    students.Add(_studentEntity.MakeStudent(grade));
                }
            }
            await UniTask.CompletedTask;
        }

        // virtual(仮想)メソッドとして定義。必要なら派生クラスで上書き(override)できる
        public virtual List<Student> GetStartingMembers()
        {
            var startingMembers = new List<Student>();
            if (students == null || !students.Any())
            {
                return startingMembers;
            }

            var availableStudents = new List<Student>(students);

            // 1. 各ポジションから最も能力値の高い選手を1人ずつ選出する
            var positions = Enum.GetValues(typeof(BasketballPosition)).Cast<BasketballPosition>();
            foreach (var position in positions)
            {
                var bestPlayerForPosition = availableStudents
                    .Where(s => s.BasketballPosition == position)
                    .OrderByDescending(s => s.Power + s.Stamina + s.Fate)
                    .FirstOrDefault();

                if (bestPlayerForPosition != null)
                {
                    startingMembers.Add(bestPlayerForPosition);
                    availableStudents.Remove(bestPlayerForPosition);
                }
            }

            // 2. 5人に満たない場合、残りの選手から能力値の高い順に補充する
            int remainingSlots = 5 - startingMembers.Count;
            if (remainingSlots > 0)
            {
                var remainingBestPlayers = availableStudents
                    .OrderByDescending(s => s.Power + s.Stamina + s.Fate)
                    .Take(remainingSlots);
                startingMembers.AddRange(remainingBestPlayers);
            }

            // 最終的に5人になるように調整し、能力値順で返す
            return startingMembers.OrderByDescending(s => s.Power + s.Stamina + s.Fate).Take(5).ToList();
        }
    }
}