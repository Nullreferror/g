using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DeckSwipe.CardModel.DrawQueue;
using DeckSwipe.CardModel.Import.Resource;
using DeckSwipe.Gamestate;
using Outfrost;
using UnityEngine;

namespace DeckSwipe.CardModel.Import
{
    public class CollectionImporter
    {
        private readonly Sprite defaultSprite;

        public CollectionImporter(Sprite defaultSprite)
        {
            this.defaultSprite = defaultSprite;
        }

        public async Task<ImportedCards> Import()
        {
            ProtoCollection collection = LocalCollection.Fetch();
            if (collection.cards.Count == 0)
            {
                Debug.LogError("[CollectionImporter] Import from local collection returned 0 cards");
            }

            Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>();
            foreach (ProtoImage image in collection.images)
            {
                if (sprites.ContainsKey(image.id))
                {
                    Debug.LogWarning($"[CollectionImporter] Duplicate {image.id} found in Images");
                    continue;
                }
                if (image.texture == null)
                {
                    Debug.LogWarning($"[CollectionImporter] Image (id:{image.id}) has a null Texture");
                    continue;
                }

                await Task.Delay(1); // TODO: remove

                Sprite sprite = Sprite.Create(
                        image.texture,
                        new Rect(0.0f, 0.0f, image.texture.width, image.texture.height),
                        new Vector2(0.5f, 0.5f));
                sprites.Add(image.id, sprite);
            }

            Dictionary<int, Character> characters = new Dictionary<int, Character>();
            foreach (ProtoCharacter protoCharacter in collection.characters)
            {
                if (characters.ContainsKey(protoCharacter.id))
                {
                    Debug.LogWarning($"[CollectionImporter] Duplicate {protoCharacter.id} found in Characters");
                    continue;
                }
                Character character = new Character(protoCharacter.name, defaultSprite);
                sprites.TryGetValue(protoCharacter.imageId, out character.sprite);
                characters.Add(protoCharacter.id, character);
            }

            Dictionary<int, Card> cards = new Dictionary<int, Card>();
            foreach (ProtoCard protoCard in collection.cards)
            {
                if (cards.ContainsKey(protoCard.id))
                {
                    Debug.LogWarning($"[CollectionImporter] Duplicate {protoCard.id} found in Cards");
                    continue;
                }

                IFollowup leftActionFollowup = null;
                ProtoAction leftAction = protoCard.leftAction;
                if (leftAction.followup != null && leftAction.followup.Count > 0)
                {
                    if (leftAction.followup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] Card (id:{protoCard.id}) left action has more than one Followup");
                        continue;
                    }
                    leftActionFollowup = leftAction.followup[0];
                }
                if (leftAction.specialFollowup != null && leftAction.specialFollowup.Count > 0)
                {
                    if (leftAction.specialFollowup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] Card (id:{protoCard.id}) left action has more than one SpecialFollowup");
                        continue;
                    }
                    if (leftActionFollowup != null)
                    {
                        Debug.LogWarning($"[CollectionImporter] Card (id:{protoCard.id}) left action has both types of followups");
                        continue;
                    }
                    leftActionFollowup = leftAction.specialFollowup[0];
                }

                IFollowup rightActionFollowup = null;
                ProtoAction rightAction = protoCard.rightAction;
                if (rightAction.followup != null && rightAction.followup.Count > 0)
                {
                    if (rightAction.followup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] Card (id:{protoCard.id}) right action has more than one Followup");
                        continue;
                    }
                    rightActionFollowup = rightAction.followup[0];
                }
                if (rightAction.specialFollowup != null && rightAction.specialFollowup.Count > 0)
                {
                    if (rightAction.specialFollowup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] Card (id:{protoCard.id}) right action has more than one SpecialFollowup");
                        continue;
                    }
                    if (rightActionFollowup != null)
                    {
                        Debug.LogWarning($"[CollectionImporter] Card (id:{protoCard.id}) right action has both types of followups");
                        continue;
                    }
                    rightActionFollowup = rightAction.specialFollowup[0];
                }

                ActionOutcome leftActionOutcome = new ActionOutcome(new StatsModification(leftAction.statsModification, leftAction.money), leftActionFollowup);
                ActionOutcome rightActionOutcome = new ActionOutcome(new StatsModification(rightAction.statsModification, rightAction.money), rightActionFollowup);

                Card card = new Card(
                        protoCard.cardText,
                        leftAction.text,
                        rightAction.text,
                        protoCard.onlyFollowup,
                        protoCard.count,
                        null,
                        leftActionOutcome,
                        rightActionOutcome);

                characters.TryGetValue(protoCard.characterId, out card.character);

                cards.Add(protoCard.id, card);
            }

            Dictionary<string, SpecialCard> specialCards = new Dictionary<string, SpecialCard>();
            foreach (ProtoSpecialCard protoSpecialCard in collection.specialCards)
            {
                if (protoSpecialCard.id == null)
                {
                    Debug.LogWarning("[CollectionImporter] Null id found in SpecialCards");
                    continue;
                }
                if (specialCards.ContainsKey(protoSpecialCard.id))
                {
                    Debug.LogWarning($"[CollectionImporter] Duplicate {protoSpecialCard.id} found in SpecialCards");
                    continue;
                }

                IFollowup leftActionFollowup = null;
                ProtoSpecialAction leftAction = protoSpecialCard.leftAction;
                if (leftAction.followup != null && leftAction.followup.Count > 0)
                {
                    if (leftAction.followup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] SpecialCard (id:{protoSpecialCard.id}) left action has more than one Followup");
                        continue;
                    }
                    leftActionFollowup = leftAction.followup[0];
                }
                if (leftAction.specialFollowup != null && leftAction.specialFollowup.Count > 0)
                {
                    if (leftAction.specialFollowup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] SpecialCard (id:{protoSpecialCard.id}) left action has more than one SpecialFollowup");
                        continue;
                    }
                    if (leftActionFollowup != null)
                    {
                        Debug.LogWarning($"[CollectionImporter] SpecialCard (id:{protoSpecialCard.id}) left action has both types of followups");
                        continue;
                    }
                    leftActionFollowup = leftAction.specialFollowup[0];
                }

                IFollowup rightActionFollowup = null;
                ProtoSpecialAction rightAction = protoSpecialCard.rightAction;
                if (rightAction.followup != null && rightAction.followup.Count > 0)
                {
                    if (rightAction.followup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] SpecialCard (id:{protoSpecialCard.id}) right action has more than one Followup");
                        continue;
                    }
                    rightActionFollowup = rightAction.followup[0];
                }
                if (rightAction.specialFollowup != null && rightAction.specialFollowup.Count > 0)
                {
                    if (rightAction.specialFollowup.Count > 1)
                    {
                        Debug.LogWarning($"[CollectionImporter] SpecialCard (id:{protoSpecialCard.id}) right action has more than one SpecialFollowup");
                        continue;
                    }
                    if (rightActionFollowup != null)
                    {
                        Debug.LogWarning($"[CollectionImporter] SpecialCard (id:{protoSpecialCard.id}) right action has both types of followups");
                        continue;
                    }
                    rightActionFollowup = rightAction.specialFollowup[0];
                }

                IActionOutcome leftActionOutcome = null;
                IActionOutcome rightActionOutcome = null;

                if (protoSpecialCard.id.ToLower().Contains("gameover"))
                {
                    leftActionOutcome = new GameOverOutcome();
                    rightActionOutcome = new GameOverOutcome();
                }

                SpecialCard specialCard = new SpecialCard(
                        protoSpecialCard.cardText,
                        leftAction.text,
                        rightAction.text,
                        null,
                        leftActionOutcome,
                        rightActionOutcome);

                characters.TryGetValue(protoSpecialCard.characterId, out specialCard.character);

                specialCards.Add(protoSpecialCard.id, specialCard);
            }

            return new ImportedCards(cards, specialCards);
        }

        private CardStatus CardStatusFor(string s)
        {
            foreach (CardStatus status in Enum.GetValues(typeof(CardStatus)))
            {
                if (s == Enum.GetName(typeof(CardStatus), status))
                {
                    return status;
                }
            }
            throw new ArgumentException($"No CardStatus value for \"{s}\"");
        }
    }
}
