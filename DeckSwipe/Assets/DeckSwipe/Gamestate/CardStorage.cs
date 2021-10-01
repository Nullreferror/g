using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckSwipe.CardModel;
using DeckSwipe.CardModel.Import;
using DeckSwipe.CardModel.Import.Resource;
using DeckSwipe.CardModel.Prerequisite;
using UnityEngine;

namespace DeckSwipe.Gamestate
{
    public class CardStorage
    {
        private readonly Sprite defaultSprite;

        public Dictionary<int, Card> Cards { get; private set; }
        public Dictionary<string, SpecialCard> SpecialCards { get; private set; }

        public Task CardCollectionImport { get; }

        private List<Card> drawableCards = new List<Card>();

        public CardStorage(Sprite defaultSprite)
        {
            this.defaultSprite = defaultSprite;
            CardCollectionImport = PopulateCollection();
        }

        public Card Random()
        {
            return drawableCards[UnityEngine.Random.Range(0, drawableCards.Count)];
        }

        public Card ForId(int id)
        {
            Card card;
            Cards.TryGetValue(id, out card);
            return card;
        }

        public SpecialCard SpecialCard(string id)
        {
            SpecialCard card;
            SpecialCards.TryGetValue(id, out card);
            return card;
        }

        public void ResolvePrerequisites()
        {
            foreach (Card card in Cards.Values)
            {
                card.ResolvePrerequisites(this);
                if (card.PrerequisitesSatisfied())
                {
                    AddDrawableCard(card);
                }
            }
        }

        public void AddDrawableCard(Card card)
        {
            drawableCards.Add(card);
        }

        private async Task PopulateCollection()
        {
            ImportedCards importedCards = await new CollectionImporter(defaultSprite).Import();
            Cards = importedCards.cards;
            SpecialCards = importedCards.specialCards;
        }
    }
}
