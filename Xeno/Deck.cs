using System;
using System.Linq;
using System.Collections.Generic;
using XENO.Cards;

namespace XENO
{
    public class Deck
    {
        public int Count => _cards.Count;

        public Deck()
        {
            _cards = new List<Card>
            {
                new Hero(), new Emperor(),
                new Spirit(), new Spirit(),
                new Sage(), new Sage(),
                new Aristocrat(), new Aristocrat(),
                new Death(), new Death(),
                new Maiden(), new Maiden(),
                new FortuneTeller(), new FortuneTeller(),
                new Soldier(), new Soldier(),
                new Boy(), new Boy(),
            };
        }

        public Deck(List<Card> cards)
        {
            _cards = cards;
        }

        public void SetRebirthCard()
        {
            SetRebirthCard(_cards[0]);
        }

        public void SetRebirthCard(Card card)
        {
            _cards.First(x => x is Hero).SetRebirth(card);
            _cards.Remove(card);
        }

        public Card Draw()
        {
            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }

        public void Add(List<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public void Shuffle()
        {
            for (var i = 0; i < _cards.Count; i++)
            {
                var target = new Random().Next(0, _cards.Count);
                var tmp = _cards[i];
                _cards[i] = _cards[target];
                _cards[target] = tmp;
            }
        }

        private readonly List<Card> _cards;
    }
}
