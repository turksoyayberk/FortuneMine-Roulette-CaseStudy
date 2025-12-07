using System;
using System.Collections;
using System.Collections.Generic;
using GameData.Appearance;
using GameData.Layout;
using GameData.Slot;
using UI;
using UnityEngine;

namespace GameData.Variations
{
    public abstract class RouletteVariationBase : ScriptableObject
    {
        public string displayName;

        public Sprite background;
        public SlotAppearanceConfig slotAppearance;

        public RouletteLayoutData layout;

        public List<SlotData> slotDatas;

        public float minStepDelay = 0.05f;
        public float maxStepDelay = 0.2f;
        public int minLoops = 2;
        public int maxLoops = 4;
        public abstract IEnumerator PlaySpin(List<SlotView> slots, Action<int> onComplete);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (layout != null && slotDatas != null)
            {
                if (slotDatas.Count != layout.roulettePositions.Count)
                {
                    Debug.LogWarning(
                        $"{name}: Slot count ({slotDatas.Count}) does not match layout positions ({layout.roulettePositions.Count})");
                }
            }
        }
#endif
    }
}