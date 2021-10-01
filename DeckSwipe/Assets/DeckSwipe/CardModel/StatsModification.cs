using System;
using System.Collections.Generic;
using DeckSwipe.Gamestate;

namespace DeckSwipe.CardModel
{
    [Serializable]
    public class StatsModification
    {
        public List<int> modifications { get; }
        public int money { get; }

        public StatsModification(List<int> modifications, int money)
        {
            this.modifications = modifications;
            this.money = money;
        }

        public void Perform()
        {
            // TODO: Pass through status effects
            Stats.ApplyModification(this);
        }
    }
}
