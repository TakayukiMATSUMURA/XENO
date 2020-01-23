using System;
using System.Linq;
using System.Collections.Generic;

namespace XENO
{
    using Cards;

    public class Player
    {
        public interface IController
        {
            int MakeDecision(List<int> cardNumbers);
            int MakeDecisionOnSage(List<int> cardNumbers);
            int MakeDecisionOnPublicExecution(List<int> cardNumbers);
        }

        public Player(string name) : this(name, new Random())
        {
        }

        public Player(string name, IController controller)
        {
            Name = name;
            _controller = controller;
        }

        public readonly string Name;

        public readonly List<Card> Trash = new List<Card>();

        public List<Card> Cards = new List<Card>();

        public Card LastCard;

        public bool IsGuarding => LastCard is Maiden;

        public int Power => Cards.Count == 0 ? 0 : Cards[0].Number;

        public void Recieve(Card card)
        {
            Log.Output($"プレイヤー:{ToString()}に{card.ToString()}が配られた.");
            Cards.Add(card);            
        }
        
        public void Draw(List<Card> deck)
        {
            Card card;
            if (LastCard is Sage)
            {
                var cards = deck.GetRange(0, Math.Min(deck.Count, 3));

                Log.Output($"プレイヤー:{ToString()}は賢者の効果で{string.Join(",", cards.Select(x => x.ToString()).ToArray())}を見た.");

                var number = _controller.MakeDecisionOnSage(cards.Select(x => x.Number).ToList());
                card = cards.First(x => x.Number == number);
                Card.Shuffle(deck);
            }
            else
            {
                card = deck[0];
            }

            Log.Output($"プレイヤー:{ToString()}は{card.ToString()}を引いた.");

            Cards.Add(card);
            deck.Remove(card);

            LastCard = null;
        }

        public void DoAction(Game game)
        {
            var candidates = Cards.Where(x => !(x is Hero)).Select(x => x.Number).ToList();
            var target = _controller.MakeDecision(candidates);
            LastCard = Cards.First(x => x.Number == target);
            Log.Output($"プレイヤー:{ToString()}は{LastCard}を使用.");
            Discard(LastCard);

            var opponent = game.GetOpponent(Cards[0]);
            if (!opponent.IsGuarding)
            {
                LastCard.InvokeOn(game);
            }
        }

        public void Discard(int cardNumber)
        {
            var card = Cards.FirstOrDefault(x => x.Number == cardNumber);
            Discard(card);
        }

        public void Discard(Card card)
        {
            if(card == null)
            {
                return;
            }

            Trash.Add(card);
            Cards.Remove(card);
        }

        public void ExchangeCardsWith(Player target)
        {
            var tmp = Cards;
            Cards = target.Cards;
            target.Cards = tmp;
        }

        public void Rebirth(Card newCard)
        {
            foreach(var card in Cards)
            {
                Trash.Add(card);
            }
            Cards.Clear();
            Cards.Add(newCard);
        }

        public void DuelWith(Player opponent)
        {
            if(Power >= opponent.Power)
            {
                opponent.Discard(opponent.Cards[0]);
            }
            else if(Power <= opponent.Power)
            {
                Discard(Cards[0]);
            }
        }

        public int SelectOnPublicExecution(Game game)
        {
            var opponent = game.Players.First(x => x != this);
            return _controller.MakeDecisionOnPublicExecution(opponent.Cards.Select(x => x.Number).ToList());
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(",", Cards.Select(x => x.ToString()).ToArray())})";
        }

        private class Random : IController
        {
            public int MakeDecision(List<int> cardNumbers)
            {
                return cardNumbers[new System.Random().Next(0, cardNumbers.Count)];
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

        private IController _controller;
    }
}
