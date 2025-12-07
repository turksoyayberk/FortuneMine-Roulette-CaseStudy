using GameData.Rewards;

namespace Core.Events
{
    public static class UIEvents
    {
        public readonly struct ChangedWalletEvent : IEvent
        {
            public readonly int Amount;

            public ChangedWalletEvent(int amount)
            {
                Amount = amount;
            }
        }
        
        public readonly struct RewardWonEvent : IEvent
        {
            public readonly RewardData Reward;

            public RewardWonEvent(RewardData reward)
            {
                Reward = reward;
            }
        }
    }
}