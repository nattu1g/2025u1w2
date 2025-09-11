// #if UNITY_EDITOR
// using System.IO;
// using UnityEditor;
// using UnityEngine;

// public class SOFromCSV
// {
//     [MenuItem("Tools/Import SO From CSV")]
//     public static void ImportCSV()
//     {
//         string path = EditorUtility.OpenFilePanel("Select CSV File for ClubItemCards", "", "csv");
//         if (string.IsNullOrEmpty(path)) return;

//         string[] lines = File.ReadAllLines(path);
//         if (lines.Length <= 1) return; // header only

//         string assetFolder = "Assets/ScriptableObjects/ClubItemCards";
//         if (!AssetDatabase.IsValidFolder(assetFolder))
//         {
//             // 親フォルダが存在しない場合も考慮
//             if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
//             {
//                 AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
//             }
//             AssetDatabase.CreateFolder("Assets/ScriptableObjects", "ClubItemCards");
//         }

//         for (int i = 1; i < lines.Length; i++)
//         {
//             string[] values = lines[i].Split(',');

//             // CSVの列が6列（インデックス0から5まで）あることを確認します
//             if (values.Length < 6)
//             {
//                 Debug.LogWarning($"ライン{i + 1}での無効な行：少なくとも14列が予想されますが、{values.Length}を取得しました。行：{lines[i]}");
//                 continue;
//             }

//             ClubItemCard asset = ScriptableObject.CreateInstance<ClubItemCard>();
//             try
//             {
//                 asset.id = values[0];
//                 asset.cardName = values[1];
//                 asset.readingName = values[2];
//                 asset.attack = int.Parse(values[3]);

//                 asset.sprite = Resources.Load<Sprite>(values[4]);
//                 if (asset.sprite == null)
//                 {
//                     Debug.LogWarning($"Sprite not found: {values[4]} for club item {asset.cardName}");
//                 }

//                 // asset.classType = (ClassType)System.Enum.Parse(typeof(ClassType), values[5].Trim());
//                 // asset.clubs = values[6]
//                 //     .Split('|')
//                 //     .Select(club => (Club)System.Enum.Parse(typeof(Club), club.Trim())) // Trim() を追加して前後の空白を除去
//                 //     .ToList();
//                 // asset.rarity = (Rarity)System.Enum.Parse(typeof(Rarity), values[7].Trim());
//                 // asset.tags = values[8]
//                 //     .Split('|')
//                 //     .Select(tag => (Tag)System.Enum.Parse(typeof(Tag), tag.Trim()))
//                 //     .ToList();
//                 // asset.ability1Name = values[9];
//                 // asset.ability1Description = values[10];
//                 // asset.ability2Name = values[11];
//                 // asset.ability2Description = values[12];
//                 // asset.spriteSizeIsBig = bool.Parse(values[13]);
//             }
//             catch (System.Exception ex)
//             {
//                 Debug.LogError($"Error parsing line {i + 1} for club item: {ex.Message} - Line: {lines[i]}");
//                 ScriptableObject.DestroyImmediate(asset); // エラー時はインスタンスを破棄
//                 continue;
//             }

//             string assetPath = $"{assetFolder}/{asset.id}.asset";
//             AssetDatabase.CreateAsset(asset, assetPath);
//         }

//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         Debug.Log("ClubItemCards imported successfully.");
//     }
// }
// #endif