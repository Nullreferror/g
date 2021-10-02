using DeckSwipe.Gamestate;
using Outfrost;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeckSwipe.World
{
    public class StatsDisplay : MonoBehaviour
    {
        public List<Image> bars;
        public float relativeMargin;

        private float minFillAmount;
        private float maxFillAmount;

        private void Awake()
        {
            minFillAmount = Mathf.Clamp01(relativeMargin);
            maxFillAmount = Mathf.Clamp01(1.0f - relativeMargin);

            if (!Util.IsPrefab(gameObject))
            {
                Stats.AddChangeListener(this);
                TriggerUpdate();
            }
        }

        public void TriggerUpdate()
        {
            for (int i = 0; i < bars.Count; ++i)
            {
                bars[i].fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.GetStatPercentage(i));
            }
        }
    }
}
