using System.Collections.Generic;
using UnityEngine;

namespace BBSim.SOs
{
    [CreateAssetMenu(fileName = "ClubItemList", menuName = "ScriptableObjects/ClubItemList", order = 1)]
    public class ClubItemList : ScriptableObject
    {
        public List<ClubItemCard> clubItemCardList;


    }
}
