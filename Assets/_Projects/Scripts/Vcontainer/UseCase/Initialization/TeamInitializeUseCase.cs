using BBSim.Vcontainer.Entity;
using Common.Vcontainer.UseCase.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BBSim.Vcontainer.UseCase
{
    public class TeamInitializeUseCase : IInitializableUseCase
    {
        private readonly TeamEntity _teamEntity;

        // 実行順序を100に設定 (マスターデータロードなど、より優先度の高い処理の後)
        public int Order => 100;

        public TeamInitializeUseCase(TeamEntity teamEntity)
        {
            _teamEntity = teamEntity;
        }

        /// <summary>
        /// チーム全体の初期化を行う
        /// </summary>
        public async UniTask InitializeAsync()
        {
            Debug.Log("[TeamInitializeUseCase][InitializeAsync] Start");
            const int opponentCount = 5;
            const string playerTeamName = "自チーム";

            // プレイヤーチームの初期化
            await _teamEntity.PlayerClub.GenerateStudent(3);
            _teamEntity.PlayerClub.Name = playerTeamName;

            // 対戦相手チームの初期化
            for (int i = 0; i < opponentCount; i++)
            {
                var opponent = _teamEntity.CreateNewOpponent();
                await opponent.GenerateStudent(3);
                opponent.Name = $"強豪校 {i + 1}";
                _teamEntity.AddOpponent(opponent);
            }
        }
    }
}
