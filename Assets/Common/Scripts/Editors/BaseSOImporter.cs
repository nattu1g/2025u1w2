// In: Common/Scripts/Editor/
using System.IO;
using UnityEditor;
using UnityEngine;

// <T> を使うことで、どんなScriptableObjectの型にも対応できる（ジェネリクス）
public abstract class BaseSOImporter<T> where T : ScriptableObject
{
    // 継承したクラスが、保存先フォルダを指定するための口
    protected abstract string AssetFolder { get; }

    // 継承したクラスが、CSVの1行をどう解釈するかを記述するための口
    protected abstract void ParseValues(T asset, string[] values);

    // 継承したクラスが、アセットのファイル名を決めるための口
    protected abstract string GetAssetName(T asset);

    // これが共通エンジン本体
    public void Import()
    {
        string path = EditorUtility.OpenFilePanel($"Select CSV for {typeof(T).Name}", "", "csv");
        if (string.IsNullOrEmpty(path)) return;

        if (!AssetDatabase.IsValidFolder(AssetFolder))
        {
            // フォルダ作成処理...
        }

        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            T asset = ScriptableObject.CreateInstance<T>();

            try
            {
                // 具体的な解釈は、継承したクラスのParseValuesに任せる
                ParseValues(asset, values);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing line {i + 1}: {ex.Message}");
                Object.DestroyImmediate(asset);
                continue;
            }

            // ファイル名は、継承したクラスのGetAssetNameに任せる
            string assetPath = $"{AssetFolder}/{GetAssetName(asset)}.asset";
            AssetDatabase.CreateAsset(asset, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"{typeof(T).Name} imported successfully.");
    }
}