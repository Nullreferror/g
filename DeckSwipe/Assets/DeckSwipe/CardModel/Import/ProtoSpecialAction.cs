using System;
using System.Collections.Generic;
using DeckSwipe.CardModel.DrawQueue;

namespace DeckSwipe.CardModel.Import
{
    [Serializable]
    public class ProtoSpecialAction
    {
        public string text;
        public List<Followup> followup;
        public List<SpecialFollowup> specialFollowup;
    }
}
