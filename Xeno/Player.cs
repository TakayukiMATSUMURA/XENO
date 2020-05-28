using System;
using System.Linq;
using System.Collections.Generic;
using XENO.Brains;
using XENO.Cards;

namespace XENO
{
    public class Player
    {        
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

        public Player(string name) : this(name, new AtRandom())
        {
        }

        public Player(string name, IBrain brain)
        {
            Name = name;
            _brain = brain;
            _draw = DrawOneCard;
        }

        public void Receive(Card card)
        {            
            _cards.Add(card);
            OnReceive(card);
        }

        public void OverrideNextDraw()
        {
            _draw = (deck) =>
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

        public void DoAction(Deck deck, Player opponent)
        {
            var card = _draw(deck);
            _draw = DrawOneCard;

            if (_brain is Console)
            {
                OnAdd(card);
            }

            Receive(card);

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
            var card = _cards[new Random().Next(0, _cards.Count)];
            Discard(card);
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

        public Card Card => _cards[0];
        public List<Card> Cards => new List<Card>(_cards);

        public override string ToString()
        {
            return _brain is ConsoleInput ?  $"{Name}({string.Join(",", _cards.Select(x => x.ToString()).ToArray())})" : Name;
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
