// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;

// // CommonのBaseSOImporterを、ClubItemCard型で継承
// public class ClubItemCardImporter : BaseSOImporter<ClubItemCard>
// {
//     // Unityのメニューに表示するための記述
//     [MenuItem("Tools/Import SO From CSV")]
//     public static void ShowWindow()
//     {
//         // インスタンスを作って、共通エンジン（Importメソッド）を呼び出す
//         new ClubItemCardImporter().Import();
//     }

//     // 以下、固有の部分だけを記述する
//     protected override string AssetFolder => "Assets/ScriptableObjects/ClubItemCards";

//     protected override string GetAssetName(ClubItemCard asset) => asset.id;

//     protected override void ParseValues(ClubItemCard asset, string[] values)
//     {
//         // ここに、BBSim固有のCSV解釈ロジックだけを書く
//         asset.id = values[0];
//         asset.cardName = values[1];
//         asset.readingName = values[2];
//         asset.attack = int.Parse(values[3]);
//         asset.sprite = Resources.Load<Sprite>(values[4]);
//     }
// }
// #endif
