using System; // Actionのため
using UnityEngine;

namespace Common.Features.Save
{
    /// <summary>
    /// SaveManagerはシーンにコンポーネントとして設置する。
    /// </summary>
    public class SaveManager : MonoBehaviour
    {

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void SaveToIndexedDB(string key, string value);
    [DllImport("__Internal")] private static extern void LoadFromIndexedDB(string key, string gameObjectName, string callbackMethodName); // callbackMethodName を受け取るように
    [DllImport("__Internal")] private static extern void DeleteFromIndexedDB(string key);
    [DllImport("__Internal")] private static extern void ClearIndexedDB();
#endif

        // --- イベント ---
        // ロード完了時にキーとデータを通知するイベント
        public event Action<string, string> OnDataReady;
        // --- End イベント ---

        private string _currentlyLoadingKey; // どのキーをロード中か一時的に保持 (ReceiveGenericDataFromJS用)

        // UseCaseから呼ばれる汎用保存メソッド
        public void SaveData(string key, string value)
        {
            // Debug.Log($"[SaveManager][SaveData] WEBGL 保存しようとしています. Key: {key}");
#if UNITY_WEBGL && !UNITY_EDITOR
            SaveToIndexedDB(key, value);
#endif
        }
        public void SaveData<T1, T2>(string path, SaveValue<T1, T2?> value)
        {
            // Debug.Log($"[SaveManager-Editor][SaveData] Not WEBGL Save: {path} = {value}");
            SaveDataManager.SaveJsonData(value, path);
        }

        // 内部用、または特定のコールバックを指定したい場合に使用できるLoadData
        public void LoadData(string key)
        {
            // Debug.Log($"[SaveManager] Attempting to load. Key: {key}");
#if UNITY_WEBGL && !UNITY_EDITOR
            _currentlyLoadingKey = key; // JavaScriptからのコールバック時にキーを特定するため
            LoadFromIndexedDB(key, gameObject.name, "ReceiveGenericDataFromJS"); // コールバックメソッド名を変更
            // Debug.Log($"[SaveManager-WebGL] LoadFromIndexedDB called for key: {key}");
#endif
        }

        public void LoadData<T1, T2>(string path, SaveValue<T1, T2?> value)
        {
            SaveDataManager.LoadJsonData(value, path);
        }
        // JavaScriptから呼び出される汎用コールバック関数
        public void ReceiveGenericDataFromJS(string jsonData) // public にし、コメント解除
        {
            if (string.IsNullOrEmpty(_currentlyLoadingKey))
            {
                Debug.LogError("[SaveManager] ReceiveGenericDataFromJS called but _currentlyLoadingKey is not set. This might happen if LoadData was not called before JS callback or if multiple loads are interleaved.");
                return;
            }
            // Debug.Log($"[SaveManager] Generic data received from JS for key '{_currentlyLoadingKey}': {(string.IsNullOrEmpty(jsonData) ? "null" : "data present")}");
            OnDataReady?.Invoke(_currentlyLoadingKey, jsonData);
        }
    }
}
