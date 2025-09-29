using UnityEngine;

namespace App.SOs
{
    [CreateAssetMenu(fileName = "ClubItemCard", menuName = "ScriptableObjects/ClubItemCard", order = 1)]
    public class ClubItemCard : ScriptableObject
    {
        public string id;
        public string cardName;
        public string readingName;
        public int attack;
        public Sprite sprite;
        // public ClassType classType;
        // public List<Club> clubs;
        // public Rarity rarity;
        // public List<Tag> tags;
        // public string ability1Name;
        // public string ability1Description;
        // public string ability2Name;
        // public string ability2Description;
        // public bool spriteSizeIsBig;
    }
}
