using Common.Features.Save;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Vcontainer.UseCase.SaveLoad
{
    public abstract class BaseSaveUseCase
    {
        protected readonly SaveManager SaveManager;

        protected BaseSaveUseCase(SaveManager saveManager)
        {
            SaveManager = saveManager;
        }

        // 具体的なセーブ処理は継承したクラスに任せる (abstract)
        public abstract UniTask SaveAllDataAsync();
    }
}
