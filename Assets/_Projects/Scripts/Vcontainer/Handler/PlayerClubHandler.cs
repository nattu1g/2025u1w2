using Scripts.Features;
using Scripts.Features.Status;
using Scripts.UI.Core;
using Scripts.Vcontainer.Entity;
using Scripts.Vcontainer.Handler;
using UnityEngine;

namespace Scripts.Vcontainer.Handler
{
    public class PlayerClubHandler : IHandler
    {
        readonly StudentEntity _studentEntity;
        readonly PlayerClubEntity _playerClubEntity;
        readonly ComponentAssembly _componentAssembly;
        readonly UICanvas _uiCanvas;



        public PlayerClubHandler(
            StudentEntity studentEntity,
            PlayerClubEntity playerClubEntity,
            ComponentAssembly componentAssembly,
            UICanvas uiCanvas
            )
        {
            _studentEntity = studentEntity;
            _playerClubEntity = playerClubEntity;
            _componentAssembly = componentAssembly;
            _uiCanvas = uiCanvas;
        }

        private const int gradeMemberCount = 3;
        public void Initialize()
        {
            for (int i = 0; i < gradeMemberCount; i++)
            {
                // とりあえず１〜３年まで一人づつ作成
                _playerClubEntity.Students.Add(_studentEntity.MakeStudent(1));
                _playerClubEntity.Students.Add(_studentEntity.MakeStudent(2));
                _playerClubEntity.Students.Add(_studentEntity.MakeStudent(3));
            }

            foreach (var item in _playerClubEntity.Students)
            {
                // Debug.Log(item.ToString());
                var obj = _componentAssembly.MakeGameObject(_componentAssembly.StatusCardPrefab, _uiCanvas.PlayerStatusView.StatusCardContainer.transform);
                var statusCard = obj.GetComponent<StatusCard>();
                statusCard.SetStatus(item);
                // statusCard.Name.text = item.Name;
                // statusCard.Stamina.text = item.Stamina.ToString();
                // statusCard.Power.text = item.Power.ToString();
                // statusCard.Fate.text = item.Fate.ToString();

            }
        }

        public void Clear()
        {
        }

        public void Dispose()
        {
        }
    }
}
