using System;
using System.Collections.Generic;
using VContainer;

namespace BBSim.Vcontainer.Entity
{
    public class TeamEntity
    {
        // private readonly Func<OpponentClubEntity> _opponentFactory;
        private readonly IObjectResolver _resolver;

        public PlayerClubEntity PlayerClub { get; }
        private readonly List<OpponentClubEntity> _opponentClubs = new();
        public IReadOnlyList<OpponentClubEntity> OpponentClubs => _opponentClubs;

        public TeamEntity(
            PlayerClubEntity playerClub,
            // Func<OpponentClubEntity> opponentFactory
            IObjectResolver resolver
            )
        {
            PlayerClub = playerClub;
            // _opponentFactory = opponentFactory;
            _resolver = resolver;
        }

        /// <summary>
        /// DIコンテナに登録されたFactoryを使って、新しい対戦相手を生成します。
        /// </summary>
        public OpponentClubEntity CreateNewOpponent()
        {
            // return _opponentFactory();
            return _resolver.Resolve<OpponentClubEntity>();
        }

        /// <summary>
        /// 対戦相手をリストに追加します。
        /// </summary>
        public void AddOpponent(OpponentClubEntity opponent)
        {
            _opponentClubs.Add(opponent);
        }
    }
}
