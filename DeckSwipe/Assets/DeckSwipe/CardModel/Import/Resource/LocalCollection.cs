using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DeckSwipe.CardModel.Import;
using Outfrost;
using UnityEngine;

namespace DeckSwipe.CardModel.Import.Resource
{
    public static class LocalCollection
    {
        private const string _cardsPath = "Collection/Cards";
        private const string _specialCardsPath = "Collection/SpecialCards";
        private const string _charactersPath = "Collection/Characters";
        private const string _imagesPath = "Collection/Images";

        public static ProtoCollection Fetch()
        {
            var cards = JsonResources.Load<ProtoCard>(_cardsPath);
            var specialCards = JsonResources.Load<ProtoSpecialCard>(_specialCardsPath);
            var characters = JsonResources.Load<ProtoCharacter>(_charactersPath);
            var images = LoadImages(_imagesPath);

            Debug.Log("[LocalCollection] Loaded " + cards.Count + " cards");
            Debug.Log("[LocalCollection] Loaded " + specialCards.Count + " special cards");
            Debug.Log("[LocalCollection] Loaded " + characters.Count + " characters");
            Debug.Log("[LocalCollection] Loaded " + images.Count + " images");

            return new ProtoCollection(cards, specialCards, characters, images);
        }

        private static List<ProtoImage> LoadImages(string path)
        {
            var list = new List<ProtoImage>();

            var assets = Resources.LoadAll<Texture2D>(path);
            foreach (var asset in assets)
            {
                list.Add(
                    new ProtoImage
                    {
                        id = int.Parse(asset.name),
                        texture = asset
                    });
            }

            return list;
        }
    }
}
