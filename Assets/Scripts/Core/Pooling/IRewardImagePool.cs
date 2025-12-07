using UI;
using UnityEngine;

namespace Core.Pooling
{
    public interface IRewardImagePool
    {
        RewardImage GetRewardImage(Vector3 position);
    }
}