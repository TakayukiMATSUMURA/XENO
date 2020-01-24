using System;
using System.Linq;
using System.Collections.Generic;

namespace XENO
{
    using Cards;

    public class Player
    {
        public interface IBrain
        {
            int MakeDecision(List<int> cardNumbers);
            int MakeDecisionOnSoldier();
            int MakeDecisionOnSage(List<int> cardNumbers);
            int MakeDecisionOnPublicExecution(List<int> cardNumbers);
        }

        public class Console : IBrain
        {
            public int MakeDecision(List<int> cardNumbers)
            {
                return GetNumberFromReadLine(cardNumbers);
            }

            public int MakeDecisionOnSoldier()
            {
                return GetNumberFromReadLine(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            }

            public int MakeDecisionOnPublicExecution(List<int> cardNumbers)
            {
                return GetNumberFromReadLine(cardNumbers);
            }

            public int MakeDecisionOnSage(List<int> cardNumbers)
            {
                return GetNumberFromReadLine(cardNumbers);
            }

            private int GetNumberFromReadLine(List<int> cardNumbers)
            {
                var result = 0;
                var input = string.Empty;
                while (!cardNumbers.Contains(result))
                {
                    try
                    {
                        Log.Output($"選択するカードの番号を入力してください:({string.Join(" or ", cardNumbers.ToArray())}).");
                        input = System.Console.ReadLine();
                        result = int.Parse(input);
                    }
                    catch (Exception)
                    {
                        Log.Output($"入力が不正:{input}.");
                    }
                }
                return result;
            }
        }

        public readonly string Name;

        public readonly List<Card> Trash = new List<Card>();

        public bool Used<T>() where T : Card => _lastCard is T;

        public int Power => _cards.Count == 0 ? 0 : _cards[0].Number;

        public bool Has(Card card) => _cards.Contains(card) || Trash.Contains(card) || _lastCard == card;

        public bool IsAlive => _cards.Count > 0;

        public Player(string name) : this(name, new Random())
        {
        }

        public Player(string name, IBrain brain)
        {
            Name = name;
            _brain = brain;
        }

        public void Recieve(Card card)
        {
            Log.Output($"プレイヤー:{ToString()}に{card.ToString()}が配られた.");
            _cards.Add(card);
        }

        public void Draw(List<Card> deck)
        {
            Card card;
            if (_lastCard is Sage)
            {
                var cards = deck.GetRange(0, Math.Min(deck.Count, 3));

                Log.Output($"プレイヤー:{ToString()}は賢者の効果で{string.Join(",", cards.Select(x => x.ToString()).ToArray())}を見た.");

                var number = _brain.MakeDecisionOnSage(cards.Select(x => x.Number).ToList());
                card = cards.First(x => x.Number == number);
                Card.Shuffle(deck);
            }
            else
            {
                card = deck[0];
            }

            Log.Output($"プレイヤー:{ToString()}は{card.ToString()}を引いた.");

            _cards.Add(card);
            deck.Remove(card);

            _lastCard = null;
        }

        // 死神の効果
        public void DrawAndDiscard(Card card)
        {
            Log.Output($"プレイヤー:{ToString()}は死神の効果で{card.ToString()}を引いた.");

            _cards.Add(card);

            var target = _cards[new System.Random().Next(0, _cards.Count)];
            Discard(target.Number, false);
        }

        // 公開処刑
        public List<Card> DrawAndRevealCards(Card card)
        {
            Log.Output($"プレイヤー:{ToString()}は公開処刑の効果で{card.ToString()}を引いた.");
            _cards.Add(card);
            return _cards;
        }

        public void DoAction(Game game)
        {
            var candidates = _cards.Where(x => !(x is Hero)).Select(x => x.Number).Distinct().ToList();
            var target = candidates.Count == 1 ? candidates[0] : _brain.MakeDecision(candidates);
            _lastCard = _cards.First(x => x.Number == target);
            Log.Output($"プレイヤー:{ToString()}は{_lastCard}を使用.");
            Discard(_lastCard);

            var opponent = game.GetOpponent(_cards[0]);
            if (!opponent.Used<Maiden>())
            {
                _lastCard.InvokeOn(game);
            }
        }

        public void Discard(int cardNumber, bool byEmperor = false)
        {
            var card = _cards.FirstOrDefault(x => x.Number == cardNumber);
            Discard(card, byEmperor);
        }

        public void ExchangeCardsWith(Player target)
        {
            var tmp = _cards;
            _cards = target._cards;
            target._cards = tmp;
        }

        public int SelectOnSoldier()
        {
            return _brain.MakeDecisionOnSoldier();
        }

        public int SelectOnPublicExecution(Game game)
        {
            var opponent = game.GetOpponent(_cards[0]);
            return _brain.MakeDecisionOnPublicExecution(opponent._cards.Select(x => x.Number).ToList());
        }

        public Card RevealCard() => _cards[0];

        public override string ToString()
        {
            return _brain is Console ?  $"{Name}({string.Join(",", _cards.Select(x => x.ToString()).ToArray())})" : Name;
        }

        private class Random : IBrain
        {
            public int MakeDecision(List<int> cardNumbers)
            {
                return cardNumbers[new System.Random().Next(0, cardNumbers.Count)];
            }

            public int MakeDecisionOnSoldier()
            {
                return new System.Random().Next(1, 10 + 1);
            }

            public int MakeDecisionOnSage(List<int> cardNumbers)
            {
                return cardNumbers[new System.Random().Next(0, cardNumbers.Count)];
            }

            public int MakeDecisionOnPublicExecution(List<int> cardNumbers)
            {
                return cardNumbers[new System.Random().Next(0, cardNumbers.Count)];
            }
        }

        private IBrain _brain;

        private List<Card> _cards = new List<Card>();

        private Card _lastCard;

        private void Discard(Card card, bool byEmperor = false)
        {
            if (card == null)
            {
                return;
            }

            Log.Output($"プレイヤー:{ToString()}は{card.ToString()}を捨てた.");

            Trash.Add(card);
            _cards.Remove(card);

            if (card is Hero)
            {
                if (_cards.Count > 0)
                {
                    Discard(_cards[0]);
                }
                if (!byEmperor)
                {
                    _cards.Add(card.BeReborn());
                }
            }
        }
    }
}
