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

        private readonly List<Card> _trash = new List<Card>();
        public List<Card> Trash => new List<Card>(_trash);

        public int Power => _cards.Count == 0 ? 0 : _cards[0].Number;

        public bool IsAlive => _cards.Count > 0;

        public bool IsGuarding { get; private set; }

        public event Action<Card> OnReceive = delegate { };
        public event Action<Card> OnAdd = delegate { };
        public event Action<List<Card>> OnSage = delegate { };
        public event Action<Card> OnUse = delegate { };
        public event Action<Card> OnDiscard = delegate { };
        public event Action<Card> OnRebirth = delegate { };

        public Player(string name) : this(name, new Random())
        {
        }

        public Player(string name, IBrain brain)
        {
            Name = name;
            _brain = brain;
            _draw = DrawOneCard;
        }

        public void Recieve(Card card)
        {            
            _cards.Add(card);
            OnReceive(card);
        }

        public void OnUseSage()
        {
            _draw -= DrawOneCard;
            _draw += (deck) =>
            {
                var cards = new List<Card>();
                for (var i = 0; i < 3 && deck.Count > 0; i++)
                {
                    cards.Add(deck.Draw());
                }

                OnSage(cards);

                var number = _brain.MakeDecisionOnSage(cards.Select(x => x.Number).ToList());
                var card = cards.First(x => x.Number == number);
                cards.Remove(card);
                deck.Add(cards);
                deck.Shuffle();
                return card;
            };
        }

        public void Guard()
        {
            IsGuarding = true;
        }

        // 公開処刑
        public void DrawAndDiscardByOpponent(Deck deck, Player opponent, bool byEmperor)
        {
            if (deck.Count == 0)
            {
                return;
            }

            var card = deck.Draw();
            _cards.Add(card);
            OnAdd(card);

            var cardNumber = _cards.Select(x => x.Number).Distinct().Count() == 1 ? _cards[0].Number : byEmperor && _cards.Any(x => x is Hero) ? 10 : opponent.SelectOnPublicExecution(this);
            Discard(cardNumber, byEmperor);
        }

        public void DoAction(Deck deck, Player opponent)
        {
            var card = _draw(deck);
            _draw = DrawOneCard;

            if (_brain is Console)
            {
                OnAdd(card);
            }

            _cards.Add(card);

            IsGuarding = false;

            var candidates = _cards.Where(x => !(x is Hero)).Select(x => x.Number).Distinct().ToList();
            var target = candidates.Count == 1 ? candidates[0] : _brain.MakeDecision(candidates);
            var selectedCard = _cards.First(x => x.Number == target);
            OnUse(selectedCard);
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

        public void DiscardAtRandom()
        {
            var cardNumber = _cards[new System.Random().Next(0, _cards.Count)].Number;
            Discard(cardNumber);
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

        private Func<Deck, Card> _draw;

        private Card DrawOneCard(Deck deck) => deck.Draw();

        private void Discard(Card card, bool byEmperor = false)
        {
            if (card == null)
            {
                return;
            }

            _trash.Add(card);
            _cards.Remove(card);

            OnDiscard(card);

            if (card is Hero)
            {
                if (_cards.Count > 0)
                {
                    Discard(_cards[0]);
                }
                if (!byEmperor)
                {
                    var rebirthCard = card.BeReborn();
                    _cards.Add(rebirthCard);
                    OnRebirth(rebirthCard);
                }
            }
        }
    }
}
