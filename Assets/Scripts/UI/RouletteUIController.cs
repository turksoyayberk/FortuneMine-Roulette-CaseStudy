using GameData.Variations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RouletteUIController : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI titleText;

        public void Initialize(RouletteVariationBase variation)
        {
            if (backgroundImage != null)
                backgroundImage.sprite = variation.background;

            if (titleText != null)
                titleText.text = variation.displayName;
        }
    }
}