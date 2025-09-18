using BBSim.Features.Status;
using BBSim.UIs.Core;
using BBSim.Vcontainer.Entity;
using Common.Events;
using Common.Features;
using MessagePipe;
using VContainer.Unity;

namespace BBSim.Vcontainer.Presenter
{
    public class PlayerClubPresenter : IStartable
    {
        private readonly ComponentAssembly _componentAssembly;
        private readonly UICanvas _uiCanvas;
        private readonly TeamEntity _teamEntity; // PlayerClubEntityの代わりにTeamEntityを注入

        // コンストラクタで依存性を注入し、イベントを購読する
        public PlayerClubPresenter(
            ComponentAssembly componentAssembly,
            UICanvas uiCanvas,
            TeamEntity teamEntity,
            ISubscriber<GameInitializedEvent> subscriber) // イベント購読用のISubscriberを注入
        {
            _componentAssembly = componentAssembly;
            _uiCanvas = uiCanvas;
            _teamEntity = teamEntity;

            // ゲームのデータ初期化が完了したことを通知するイベントを購読する
            subscriber.Subscribe(_ => OnGameInitialized());
        }

        public void Start()
        {
        }

        // ゲームの初期化が完了したときに呼び出されるメソッド
        private void OnGameInitialized()
        {
            // TeamEntityからプレイヤーのクラブ情報を取得
            var playerClub = _teamEntity.PlayerClub;

            // 取得したデータをもとにUIを生成・更新する
            foreach (var student in playerClub.Students)
            {
                var obj = _componentAssembly.MakeGameObject(_componentAssembly.StatusCardPrefab, _uiCanvas.PlayerStatusView.StatusCardContainer.transform);
                var statusCard = obj.GetComponent<StatusCard>();
                statusCard.SetStatus(student);
            }
        }
    }
}
