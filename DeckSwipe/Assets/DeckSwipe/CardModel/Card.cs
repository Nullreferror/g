using System.Collections.Generic;
using DeckSwipe.Gamestate;
using UnityEngine;

namespace DeckSwipe.CardModel
{
    public class Card : ICard
    {
        public string CardText { get; }
        public string LeftSwipeText { get; }
        public string RightSwipeText { get; }

        public string CharacterName
        {
            get { return character != null ? character.name : ""; }
        }

        public Sprite CardSprite
        {
            get { return character?.sprite; }
        }

        public ICardProgress Progress
        {
            get { return progress; }
        }

        public Character character;
        public CardProgress progress;

        private readonly ActionOutcome leftSwipeOutcome;
        private readonly ActionOutcome rightSwipeOutcome;

        public Card(
                string cardText,
                string leftSwipeText,
                string rightSwipeText,
                Character character,
                ActionOutcome leftOutcome,
                ActionOutcome rightOutcome)
        {
            this.CardText = cardText;
            this.LeftSwipeText = leftSwipeText;
            this.RightSwipeText = rightSwipeText;
            this.character = character;
            leftSwipeOutcome = leftOutcome;
            rightSwipeOutcome = rightOutcome;
        }

        public void CardShown(Game controller)
        {
            progress.Status |= CardStatus.CardShown;
        }

        public void PerformLeftDecision(Game controller)
        {
            progress.Status |= CardStatus.LeftActionTaken;
            leftSwipeOutcome.Perform(controller);
        }

        public void PerformRightDecision(Game controller)
        {
            progress.Status |= CardStatus.RightActionTaken;
            rightSwipeOutcome.Perform(controller);
        }
    }
}
