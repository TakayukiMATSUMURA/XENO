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

        public int Power => _cards.Count == 0 ? 0 : _cards[0].Number;

        public bool IsAlive => _cards.Count > 0;

        public bool IsGuarding { get; private set; }

        public Func<List<Card>, Card> Draw;

        public Player(string name) : this(name, new Random())
        {
        }

        public Player(string name, IBrain brain)
        {
            Name = name;
            _brain = brain;
            Draw = DrawOneCard;
        }

        public void Recieve(Card card)
        {
            Log.Output($"プレイヤー:{ToString()}に{card.ToString()}が配られた.");
            _cards.Add(card);
        }

        public Card DrawOneCard(List<Card> deck) => deck[0];

        public void OnUseSage()
        {
            Draw -= DrawOneCard;
            Draw += (deck) =>
            {
                var cards = deck.GetRange(0, Math.Min(deck.Count, 3));

                Log.Output($"プレイヤー:{ToString()}は賢者の効果で{string.Join(",", cards.Select(x => x.ToString()).ToArray())}を見た.");

                var number = _brain.MakeDecisionOnSage(cards.Select(x => x.Number).ToList());
                var card = cards.First(x => x.Number == number);
                Card.Shuffle(deck);
                return card;
            };
        }

        public void OnUseMaiden()
        {
            IsGuarding = true;
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

        public void DoAction(List<Card> deck, Player opponent)
        {
            var card = Draw(deck);
            Draw = DrawOneCard;

            if (_brain is Console)
            {
                Log.Output($"プレイヤー:{ToString()}は{card.ToString()}を引いた.");
            }

            _cards.Add(card);
            deck.Remove(card);

            IsGuarding = false;

            var candidates = _cards.Where(x => !(x is Hero)).Select(x => x.Number).Distinct().ToList();
            var target = candidates.Count == 1 ? candidates[0] : _brain.MakeDecision(candidates);
            var selectedCard = _cards.First(x => x.Number == target);
            Log.Output($"プレイヤー:{ToString()}は{selectedCard}を使用.");
            Discard(selectedCard);

            var args = new Card.InvokeArgments
            {
                Invoker = this,
                Opponent = opponent,
                Deck = deck
            };
            selectedCard.InvokeOn(args);
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

        public int SelectOnPublicExecution(Player opponent)
        {
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
