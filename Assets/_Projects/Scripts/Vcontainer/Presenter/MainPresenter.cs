using AudioConductor.Runtime.Core.Models;
using App.Vcontainer.UseCase;
using Common.Features;
using Common.Vcontainer.Entity;
using Common.Vcontainer.Handler;
using Common.Vcontainer.UseCase.Audio;
using MessagePipe;
using UnityEngine;
using VContainer.Unity;

namespace App.Vcontainer.Presenter
{

    public class MainPresenter : IInitializable, ITickable
    {
        // RootLifetimeScopeで登録されたコンポーネント
        readonly ComponentAssembly _componentAssembly;

        public MainPresenter(
            ComponentAssembly componentAssembly
            )
        {
            _componentAssembly = componentAssembly;
        }
        // InputAction inputAction;
        // CancellationToken cancellationToken;
        public void Initialize()
        {
            Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);

            // ★他のHandlerの初期化処理はInitializationOrchestratorに移動したため、ここでは不要

            // MainPresenterが直接担当する初期化処理のみを残す
            // await _audioEntity.PlayBGM("MainBGM");
        }

        public void Tick()
        {
            // Pub Example
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     _publisher.Publish(Random.Range(10, 200));
            // }

            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     Debug.Log("Qキーが押されました");
            //     _messagePublisher.Publish(new MessageEvent("Qキーが押されました", false, false));
            // }
            // if (Input.GetKeyDown(KeyCode.W))
            // {
            //     Debug.Log("Wキーが押されました");
            //     _messagePublisher.Publish(new MessageEvent("Wキーが押されましたｆだｆだｓｇｇｇｇｇｇｇｇｇ", false, true));
            // }
            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     Debug.Log("Eキーが押されました");
            //     _messagePublisher.Publish(new MessageEvent("Eキーが押されたんですけどおおおお", true, false));
            // }
            // if (Input.GetKeyDown(KeyCode.R))
            // {
            //     Debug.Log("Rキーが押されました");
            //     _messagePublisher.Publish(new MessageEvent("Rキーが押", true, true));
            // }
        }
    }
}
