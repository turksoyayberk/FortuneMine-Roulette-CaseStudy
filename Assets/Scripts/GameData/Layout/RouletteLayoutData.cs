using System.Collections.Generic;
using UI;
using UnityEngine;

namespace GameData.Layout
{
    [CreateAssetMenu(menuName = "Roulette/Layout Data")]
    public class RouletteLayoutData : ScriptableObject
    {
        public SlotView slotPrefab;
        public List<Vector2Int> roulettePositions;

        public float spacingX;
        public float spacingY;

        //public Vector2 slotScale = Vector2.one;
    }
}