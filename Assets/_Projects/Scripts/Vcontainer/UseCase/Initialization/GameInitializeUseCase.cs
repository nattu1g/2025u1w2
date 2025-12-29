using Common.Vcontainer.UseCase.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Vcontainer.UseCase.Initialization
{
    /// <summary>
    /// ゲームの初期化を管理するUseCase
    /// </summary>
    public class GameInitializeUseCase : IInitializableUseCase
    {
        // private readonly BattleUseCase _battleUseCase;
        // private readonly GlobalAssetAssembly _globalAssetAssembly;
        // private readonly App.UIs.Core.UIToolkitCanvas _uiCanvas;

        // 初期化順序（他のUseCaseより後に実行）
        public int Order => 100;

        // public GameInitializeUseCase(
        //     BattleUseCase battleUseCase,
        //     GlobalAssetAssembly globalAssetAssembly,
        //     App.UIs.Core.UIToolkitCanvas uiCanvas)
        // {
        //     _battleUseCase = battleUseCase;
        //     _globalAssetAssembly = globalAssetAssembly;
        //     _uiCanvas = uiCanvas;
        // }



        /// <summary>
        /// ゲーム初期化
        /// </summary>
        public UniTask InitializeAsync()
        {
            Debug.Log("━━━━━━━━━━━━━━━━━━━━━━");
            Debug.Log("ゲーム初期化開始");
            Debug.Log("━━━━━━━━━━━━━━━━━━━━━━");

            // ステージ選択画面を表示
            Debug.Log("ステージ選択画面を表示しました");

            return UniTask.CompletedTask;
        }
    }
}
