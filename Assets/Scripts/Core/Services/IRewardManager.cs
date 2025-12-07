using UnityEngine;

namespace Core.Services
{
    public interface IRewardManager
    {
        void PlayRewardSequence(Sprite rewardSprite, int amount, int count = 10);
    }
}