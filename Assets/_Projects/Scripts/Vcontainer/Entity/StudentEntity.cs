using BBSim.Models;
using UnityEngine;

namespace BBSim.Vcontainer.Entity
{
    public class StudentEntity
    {
        public Student MakeStudent(int grade)
        {
            string name = GetRandomName();
            // int grade = Random.Range(1, 4);

            // 学年ごとに補正を設定
            int baseMin = 10 + (grade - 1) * 10; // 1年:10, 2年:20, 3年:30
            int baseMax = 30 + (grade - 1) * 10; // 1年:30, 2年:40, 3年:50
            return new Student(name, grade, baseMin, baseMax);
        }
        private string GetRandomName()
        {
            string[] names = {
                // 日本人の苗字（カタカナ）
                "タカハシ", "ヤマモト", "サトウ", "コバヤシ", "フジタ",
                "ナカムラ", "イノウエ", "ハヤシ", "クドウ", "オオタ",
                "サイトウ", "ワタナベ", "マツモト", "カワムラ", "ヤマグチ",
                "ミヤザキ", "アベ", "ハセガワ", "イシカワ", "モリ",
                "キムラ", "サカモト", "マツイ", "ヤマダ", "オカダ",
                "ホンダ", "ナガノ", "ニシムラ", "カネコ", "ノグチ",
                "フクダ", "マエダ", "カワグチ", "オオノ", "イケダ",
                "クワハラ", "ムラカミ", "ウエダ", "アライ", "ヒラノ",
                "シライ", "ミウラ", "ヨシダ", "ウチダ", "タナカ",
                "ヨコヤマ", "オクダ", "ナカジマ", "ハラ", "イワサキ",
            
                // アメリカ人風の苗字（カタカナ表記）
                "スミス", "ジョンソン", "ウィリアムズ", "ブラウン", "テイラー"
            };

            return names[Random.Range(0, names.Length)];
        }
    }
}
