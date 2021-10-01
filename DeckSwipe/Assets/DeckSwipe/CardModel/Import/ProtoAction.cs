using System;
using System.Collections.Generic;
using DeckSwipe.CardModel.DrawQueue;

namespace DeckSwipe.CardModel.Import
{
    [Serializable]
    public class ProtoAction
    {
        public string text;
        public StatsModification statsModification;
        public List<Followup> followup;
        public List<SpecialFollowup> specialFollowup;
    }
}
