using Core.Events;
using Core.Services;
using Core.Utilities;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WalletUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;

        private const int WalletFormatThreshold = 10_000;

        private void Start()
        {
            var wallet = ServiceLocator.Resolve<IWalletService>();
            amountText.SetText(wallet.Amount.FormatNumber(WalletFormatThreshold));
        }

        private void OnEnable()
        {
            EventBus.Subscribe<UIEvents.ChangedWalletEvent>(SetAmountText);
        }

        private void OnDisable()
        {
            EventBus.UnSubscribe<UIEvents.ChangedWalletEvent>(SetAmountText);
        }

        private void SetAmountText(UIEvents.ChangedWalletEvent e)
        {
            amountText.SetText($"{e.Amount.FormatNumber(WalletFormatThreshold)}");
        }
    }
}