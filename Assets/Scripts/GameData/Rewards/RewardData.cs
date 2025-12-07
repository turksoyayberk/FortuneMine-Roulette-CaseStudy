using UnityEngine;

namespace GameData.Rewards
{
    [CreateAssetMenu(menuName = "Rewards/Reward Data")]
    public class RewardData : ScriptableObject
    {
        public Sprite icon;
        public int amount;
    }
}