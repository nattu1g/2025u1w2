using BBSim.Models;
using UnityEngine;

namespace BBSim.Vcontainer.Entity
{
    public class DrawCardEntity
    {
        public DrawCard MakeMonster(int correctionValue)
        {
            string name = GetRandomMonsterName();

            return new DrawCard(name, correctionValue);
        }
        private string GetRandomMonsterName()
        {
            string[] monsterNames = {
        "ドラゴノイド", "シャドウナイト", "フレイムゴーレム", "サンダービースト", "アクアリザード",
        "ダークエルフ", "ホーリーフェニックス", "ヴァンパイアロード", "ヘルハウンド", "デススコーピオン",
        "ブリザードウルフ", "ストームワイバーン", "ブラッドスネーク", "アースタイタン", "カオスリーパー",
        "ミラージュスピリット", "ナイトメアドラゴン", "ファントムアサシン", "クリムゾンファング", "メタルミノタウロス",
        "ライトニングスパイダー", "スケルトンメイジ", "サラマンダーキング", "アイスデーモン", "ゴーストウォリアー",
        "テンペストホーク", "ワイルドグリフォン", "マッドトロール", "ソウルイーター", "ネクロマンサー",
        "ウィンドフェアリー", "バジリスク", "グレムリンロード", "アビスドラゴン", "ギガンティックアント",
        "クリスタルゴーレム", "ナイトホラー", "ディザスターバット", "サンダーバジリスク", "フロストジャイアント"
    };

            return monsterNames[Random.Range(0, monsterNames.Length)];
        }
    }
}
