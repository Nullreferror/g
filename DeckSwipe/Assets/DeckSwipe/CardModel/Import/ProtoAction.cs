using System;
using System.Collections.Generic;
using DeckSwipe.CardModel.DrawQueue;

namespace DeckSwipe.CardModel.Import
{
    [Serializable]
    public class ProtoAction
    {
        public string text;
        public int money;
        public List<int> statsModification;
        public List<Followup> followup;
        public List<SpecialFollowup> specialFollowup;
    }
}
