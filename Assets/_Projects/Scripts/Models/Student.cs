namespace Scripts.Models
{
    public class Student
    {
        public string Name;
        public int Grade;

        public int Stamina;
        public int Power;
        public int Fate;

        public bool HasSkill;

        public Student(string name, int grade, int baseMin, int baseMax)
        {
            Name = name;
            Grade = grade;

            Stamina = UnityEngine.Random.Range(baseMin, baseMax + 1);
            Power = UnityEngine.Random.Range(baseMin, baseMax + 1);
            Fate = UnityEngine.Random.Range(baseMin, baseMax + 1);

            HasSkill = false;
        }

        public override string ToString()
        {
            return $"{Name}（{Grade}年） 持久力:{Stamina} 筋力:{Power} 運命力:{Fate} スキル:{(HasSkill ? "有" : "無")}";
        }
    }
}
