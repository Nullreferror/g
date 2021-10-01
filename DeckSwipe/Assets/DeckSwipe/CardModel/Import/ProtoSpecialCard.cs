using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeckSwipe.CardModel.Import
{
    [Serializable]
    public class ProtoSpecialCard
    {
        public string id;
        public int characterId;
        public string cardText;
        public ProtoSpecialAction leftAction;
        public ProtoSpecialAction rightAction;
    }
}
