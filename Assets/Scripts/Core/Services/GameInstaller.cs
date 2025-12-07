using UnityEngine;

namespace Core.Services
{
    public class GameInstaller : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.Register<IWalletService>(new WalletService());
        }
    }
}