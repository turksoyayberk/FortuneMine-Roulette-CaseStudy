using UnityEngine;

namespace GameData.Appearance
{
    [CreateAssetMenu(menuName = "Roulette/Slot Appearance")]
    public class SlotAppearanceConfig : ScriptableObject
    {
        public Sprite normalBackground;
        public Sprite glowSprite;

        public Sprite rewardReceived;
        public Sprite completedState;
    }
}
