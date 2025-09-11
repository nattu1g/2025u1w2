// In: Common/Scripts/Vcontainer/UseCase/
using Common.Features.Save;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Common.Vcontainer.UseCase.SaveLoad
{
    public abstract class BaseLoadUseCase : IDisposable
    {
        protected readonly SaveManager SaveManager;
        private readonly Dictionary<string, UniTaskCompletionSource<string>> _loadTcs = new();

        protected BaseLoadUseCase(SaveManager saveManager)
        {
            SaveManager = saveManager;
            SaveManager.OnDataReady += HandleDataReady;
        }

        public virtual void Dispose()
        {
            SaveManager.OnDataReady -= HandleDataReady;
            foreach (var tcs in _loadTcs.Values) tcs.TrySetCanceled();
            _loadTcs.Clear();
        }

        private void HandleDataReady(string key, string jsonData)
        {
            if (_loadTcs.TryGetValue(key, out var tcs))
            {
                tcs.TrySetResult(jsonData);
                _loadTcs.Remove(key);
            }
        }

        // 汎用的なロードメソッド
        protected async UniTask<string> LoadDataAsync(string key, string path)
        {
            var tcs = new UniTaskCompletionSource<string>();
            _loadTcs[key] = tcs;

            SaveManager.LoadData(key);
            // Editor/Standaloneでは直接データを読み込む（架空のSaveValueを使う）
            var tempSaveValue = new SaveValue<string, object>(null, null);
            SaveManager.LoadData(path, tempSaveValue);
            // ここでは簡略化のため、直接ファイルを読むなどの処理が必要になる場合があります
            // この部分は実際のSaveManagerの実装に合わせて調整します
            tcs.TrySetResult(tempSaveValue.Val1);
            return await tcs.Task;
        }

        // 具体的なロード処理は継承したクラスに任せる (abstract)
        public abstract UniTask LoadAllDataAsync();
    }
}
