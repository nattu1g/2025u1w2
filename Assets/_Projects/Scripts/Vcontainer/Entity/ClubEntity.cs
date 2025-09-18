using System.Collections.Generic;
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
        public virtual async UniTask GenerateStudent(int gradeMemberCount)
        {
            for (int i = 0; i < gradeMemberCount; i++)
            {
                students.Add(_studentEntity.MakeStudent(1));
                students.Add(_studentEntity.MakeStudent(2));
                students.Add(_studentEntity.MakeStudent(3));
            }
            await UniTask.CompletedTask;
        }
    }
}