using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioConductor.Runtime.Core;
using AudioConductor.Runtime.Core.Models;
using Cysharp.Threading.Tasks;
using GekinatuPackage.SaveJson.Data;
using GekinatuPackage.SaveJson.Json;
using MessagePipe;
using Scripts.Features.Save;
using Scripts.Features;
using Scripts.Setting;
using Scripts.UI;
using Scripts.Vcontainer.Entity;
using Scripts.Vcontainer.Handler;
using Scripts.Vcontainer.UseCase;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;
using ReactiveInputSystem;
using System.Threading;
using R3;
using Scripts.UI.Core;

namespace Scripts.Vcontainer.Presenter
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
        readonly GameInitializationHandler _gameInitHandler;
        readonly AudioHandler _audioHandler;
        readonly SaveLoadHandler _saveLoadHandler;
        readonly ButtonHandler _buttonHandler;
        readonly PlayerClubHandler _playerClubHandler;
        readonly TrainingSelectHandler _trainingSelectHandler;
        // UseCase
        readonly AudioUseCase _audioUseCase;
        readonly SaveUseCase _saveUseCase;
        readonly LoadUseCase _loadUseCase;
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
            GameInitializationHandler gameInitHandler,
            PlayerClubHandler playerClubHandler,
            TrainingSelectHandler trainingSelectHandler,
            AudioHandler audioHandler,
            SaveLoadHandler saveLoadHandler,
            ButtonHandler buttonHandler,
            AudioUseCase audioUseCase,
            SaveUseCase saveUseCase,
            LoadUseCase loadUseCase,
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
            _gameInitHandler = gameInitHandler;
            _playerClubHandler = playerClubHandler;
            _trainingSelectHandler = trainingSelectHandler;
            _audioHandler = audioHandler;
            _saveLoadHandler = saveLoadHandler;
            _buttonHandler = buttonHandler;
            _audioUseCase = audioUseCase;
            _saveUseCase = saveUseCase;
            _loadUseCase = loadUseCase;
            _messageUseCase = messageUseCase;
        }
        // InputAction inputAction;
        // CancellationToken cancellationToken;
        public async void Initialize()
        {
            // inputAction = new InputAction();
            // cancellationToken = new CancellationToken();

            // inputAction.StartedAsObservable(cancellationToken)
            //     .Subscribe(x => Debug.Log("Started"));


            // inputAction.PerformedAsObservable(cancellationToken)
            //     .Subscribe(x => Debug.Log("Performed" + x));
            // inputAction.CanceledAsObservable(cancellationToken)
            //     .Subscribe(x => Debug.Log("Canceled"));
            Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);

            await _saveLoadHandler.LoadAllFile();

            await _audioHandler.Initialize();
            _gameInitHandler.Initialize();

            _playerClubHandler.Initialize();
            _trainingSelectHandler.Initialize();



            // 一覧リストの登録
            // _listParentHandler.Initialize();
            // _studentListHandler.Initialize();
            // _studentFilterHandler.Initialize();
            // _studentDetailHandler.Initialize();

            await _audioEntity.PlayBGM("MainBGM");

            // Sub Example
            // _subscriber.Subscribe((num) =>
            // {
            //     Debug.Log("[MainPresenter] random num" + num);
            // });

            // _messageUseCase.Initialize(); // 本当はここでしたくない。Handlerとかでまとめてしたいけどテスト的に
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
