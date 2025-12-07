using System.Collections.Generic;
using Core.Utilities;
using GameData.Variations;
using UI;
using UnityEngine;

namespace Gameplay.Board
{
    public class RouletteBoard : MonoBehaviour
    {
        [SerializeField] private RectTransform slotsParent;

        private RouletteVariationBase _config;
        private readonly List<SlotView> _slots = new();
        public IReadOnlyList<SlotView> Slots => _slots;

        public void Initialize(RouletteVariationBase config)
        {
            UITweenUtils.PlayEntranceAnimation(slotsParent, delay: 0.5f);
            _config = config;
            CreateSlots();
            PositionSlots();
        }

        private void CreateSlots()
        {
            foreach (var slotData in _config.slotDatas)
            {
                var slotView = Instantiate(_config.layout.slotPrefab, slotsParent);

                slotView.SetData(slotData);
                slotView.SetAppearance(_config.slotAppearance);

                _slots.Add(slotView);
            }
        }

        private void PositionSlots()
        {
            var layout = _config.layout;

            for (var i = 0; i < _slots.Count; i++)
            {
                var logicalPos = layout.roulettePositions[i];
                var uiPos = new Vector2(
                    logicalPos.x * layout.spacingX,
                    logicalPos.y * layout.spacingY
                );

                var rt = _slots[i].GetComponent<RectTransform>();
                rt.anchoredPosition = uiPos;
            }
        }
    }
}