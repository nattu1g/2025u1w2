using AudioConductor.Runtime.Core.Models;
using BBSim.UIs.Core;
using BBSim.Vcontainer.Handler;
using BBSim.Vcontainer.UseCase;
using Common.Features;
using Common.Vcontainer.Entity;
using Common.Vcontainer.Handler;
using Common.Vcontainer.UseCase.Audio;
using MessagePipe;
using UnityEngine;
using VContainer.Unity;

namespace BBSim.Vcontainer.Presenter
{

    public class MainPresenter : IInitializable, ITickable
    {
        // RootLifetimeScopeで登録されたコンポーネント
        readonly ComponentAssembly _componentAssembly;
        readonly VolumeEntity _volumeEntity;
        readonly AudioEntity _audioEntity;
        readonly AudioConductorSettings _audioConductorSettings;
        readonly CueSheetAsset _cueSheetAsset;

        // PubSub
        readonly IPublisher<int> _publisher;
        readonly ISubscriber<int> _subscriber;
        readonly IPublisher<MessageEvent> _messagePublisher;



        // MainLifetimeScopeで登録されたコンポーネント
        readonly UICanvas _uiCanvas;
        // Handler
        // readonly GameInitializationHandler _gameInitHandler;
        readonly ButtonHandler _buttonHandler;
        readonly PlayerClubHandler _playerClubHandler;
        readonly TrainingSelectHandler _trainingSelectHandler;
        // UseCase
        readonly AudioUseCase _audioUseCase;
        readonly BbsimSaveUseCase _bbsimSaveUseCase;
        readonly BbsimLoadUseCase _bbsimLoadUseCase;
        readonly MessageUseCase _messageUseCase;



        public MainPresenter(
            ComponentAssembly componentAssembly,
            VolumeEntity volumeEntity,
            AudioEntity audioEntity,
            AudioConductorSettings audioConductorSettings,
            CueSheetAsset cueSheetAsset,
            IPublisher<int> publisher,
            ISubscriber<int> subscriber,
            IPublisher<MessageEvent> messagePublisher,
            UICanvas uiCanvas,
            // GameInitializationHandler gameInitHandler,
            PlayerClubHandler playerClubHandler,
            TrainingSelectHandler trainingSelectHandler,
            ButtonHandler buttonHandler,
            AudioUseCase audioUseCase,
            BbsimSaveUseCase bbsimSaveUseCase,
            BbsimLoadUseCase bbsimLoadUseCase,
            MessageUseCase messageUseCase
            )
        {
            _componentAssembly = componentAssembly;
            _volumeEntity = volumeEntity;
            _audioEntity = audioEntity;
            _audioConductorSettings = audioConductorSettings;
            _cueSheetAsset = cueSheetAsset;
            _publisher = publisher;
            _subscriber = subscriber;
            _messagePublisher = messagePublisher;
            _uiCanvas = uiCanvas;
            _playerClubHandler = playerClubHandler;
            _trainingSelectHandler = trainingSelectHandler;
            _buttonHandler = buttonHandler;
            _audioUseCase = audioUseCase;
            _bbsimSaveUseCase = bbsimSaveUseCase;
            _bbsimLoadUseCase = bbsimLoadUseCase;
            _messageUseCase = messageUseCase;
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
