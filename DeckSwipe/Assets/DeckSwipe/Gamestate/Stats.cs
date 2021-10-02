using System.Collections.Generic;
using System.Linq;
using DeckSwipe.CardModel;
using DeckSwipe.World;
using UnityEngine;

namespace DeckSwipe.Gamestate
{
    public static class Stats
    {
        public const int Count = 8;
        public const int MaxStatValue = 100;
        public const int MaxMoneyValue = 100;

        private const int _startingStatValue = 0;
        private const int _startingMoneyValue = 0;

        private static readonly List<StatsDisplay> _changeListeners = new List<StatsDisplay>();

        private static readonly int[] stats = new int[Count];
        private static int money;

        public static int GetStat(int i) => stats.ElementAtOrDefault(i);

        public static float GetPercentage(int i) => (float)stats.ElementAtOrDefault(i) / MaxStatValue;

        public static int GetMoney() => money;

        public static void ApplyModification(StatsModification mod)
        {
            for (int i = 0; i < Count; ++i)
            {
                stats[i] = ClampValue(stats[i] + mod.modifications.ElementAtOrDefault(i));
            }
            money += mod.money;

            TriggerAllListeners();
        }

        public static void ResetStats()
        {
            ApplyStartingValues();
            TriggerAllListeners();
        }

        private static void ApplyStartingValues()
        {
            for (int i = 0; i < Count; ++i)
            {
                stats[i] = ClampValue(_startingStatValue);
            }
            money = _startingMoneyValue;
        }

        private static void TriggerAllListeners()
        {
            for (int i = 0; i < _changeListeners.Count; i++)
            {
                if (_changeListeners[i] == null)
                {
                    _changeListeners.RemoveAt(i);
                }
                else
                {
                    _changeListeners[i].TriggerUpdate();
                }
            }
        }

        public static void AddChangeListener(StatsDisplay listener)
        {
            _changeListeners.Add(listener);
        }

        private static int ClampValue(int value)
        {
            return Mathf.Clamp(value, 0, MaxStatValue);
        }
    }
}
