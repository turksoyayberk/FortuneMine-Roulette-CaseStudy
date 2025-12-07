using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace GameData.Variations
{
    [CreateAssetMenu(menuName = "Variation/Barbeque Data")]
    public class BarbequeVariation : RouletteVariationBase
    {
        private const float LoopDelayIncrease = 0.02f;
        private const float FinalDelayIncrease = 0.01f;
        private const int BlinkCount = 3;
        private const float BlinkInterval = 0.15f;

        public override IEnumerator PlaySpin(
            List<SlotView> slots,
            System.Action<int> onComplete)
        {
            var total = slots.Count;
            var currentIndex = 0;

            var finalIndex = GetRandomFinalIndex(slots);

            var delay = minStepDelay;
            var loops = Random.Range(minLoops, maxLoops + 1);

            for (var loop = 0; loop < loops; loop++)
            {
                for (var i = 0; i < total; i++)
                {
                    AnimateStep(slots, currentIndex);

                    yield return new WaitForSeconds(delay);

                    currentIndex = (currentIndex + 1) % total;
                }

                delay = Mathf.Min(delay + LoopDelayIncrease, maxStepDelay);
            }
            
            while (currentIndex != finalIndex)
            {
                AnimateStep(slots, currentIndex);

                yield return new WaitForSeconds(delay);

                currentIndex = (currentIndex + 1) % total;

                delay = Mathf.Min(delay + FinalDelayIncrease, maxStepDelay);
            }

            ResetAllSlots(slots);

            var winningSlot = slots[currentIndex];

            for (var i = 0; i < BlinkCount; i++)
            {
                winningSlot.SetSelectedState();
                yield return new WaitForSeconds(BlinkInterval);

                winningSlot.SetNormal();
                yield return new WaitForSeconds(BlinkInterval);
            }

            winningSlot.SetSelectedState();

            onComplete?.Invoke(finalIndex);
        }

        private void AnimateStep(List<SlotView> slots, int currentIndex)
        {
            var total = slots.Count;

            var previous = (currentIndex - 1 + total) % total;
            var beforePrevious = (currentIndex - 2 + total) % total;

            slots[beforePrevious].SetNormal();
            slots[previous].SetTrailState();
            slots[currentIndex].SetSelectedState();
        }

        private int GetRandomFinalIndex(List<SlotView> slots)
        {
            int final;
            var total = slots.Count;

            do
            {
                final = Random.Range(0, total);
            } while (slots[final].IsCompleted);

            return final;
        }

        private void ResetAllSlots(List<SlotView> slots)
        {
            foreach (var slot in slots)
                slot.TrySetNormal();
        }
    }
}