using UnityEngine;

namespace Core.Utilities
{
    public static class PlayerPrefsManager
    {
        private const string WalletKey = "wallet";

        public static void SetWallet(int value) => PlayerPrefs.SetInt(WalletKey, value);
        public static int GetWallet() => PlayerPrefs.GetInt(WalletKey, 0);
    }
}