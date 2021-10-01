using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeckSwipe.CardModel.Import
{
    [Serializable]
    public class ProtoCard
    {
        public int id;
        public int characterId;
        public string cardText;
        public bool onlyFollowup = false;
        public int count = 1;
        public ProtoAction leftAction;
        public ProtoAction rightAction;
    }
}
