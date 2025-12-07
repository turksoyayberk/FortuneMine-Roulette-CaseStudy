using Core.Events;
using Core.Utilities;
using UnityEngine;

namespace Core.Services
{
    public class WalletService : IWalletService
    {
        public int Amount { get; private set; }

        public WalletService()
        {
            Amount = PlayerPrefsManager.GetWallet();
        }

        public void Add(int amount)
        {
            Amount += amount;
            Save();

            EventBus.Publish(new UIEvents.ChangedWalletEvent(Amount));
        }

        public void Set(int amount)
        {
            Amount = amount;
            Save();

            EventBus.Publish(new UIEvents.ChangedWalletEvent(Amount));
        }

        private void Save()
        {
            PlayerPrefsManager.SetWallet(Amount);
        }
    }
}