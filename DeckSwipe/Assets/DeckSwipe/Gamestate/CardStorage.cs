using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckSwipe.CardModel;
using DeckSwipe.CardModel.Import;
using DeckSwipe.CardModel.Import.Resource;
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
            if (Cards.TryGetValue(id, out var card))
            {
                return card;
            }
            Debug.LogError("TryGet Card" + id + "fail!");
            return null;
        }

        public SpecialCard SpecialCard(string id)
        {
            if (SpecialCards.TryGetValue(id, out var card))
            {
                return card;
            }
            Debug.LogError("TryGet SpecialCard" + id + "fail!");
            return null;
        }

        public void FillDrawableCard()
        {
            foreach (Card card in Cards.Values)
            {
                //if (card.PrerequisitesSatisfied())
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
