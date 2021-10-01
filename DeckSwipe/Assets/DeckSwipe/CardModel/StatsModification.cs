using System;
using System.Collections.Generic;
using DeckSwipe.Gamestate;

namespace DeckSwipe.CardModel
{
    [Serializable]
    public class StatsModification
    {
        public List<int> modifications;

        public StatsModification(List<int> modifications)
        {
            this.modifications = modifications;
        }

        public void Perform()
        {
            // TODO: Pass through status effects
            Stats.ApplyModification(this);
        }
    }
}
