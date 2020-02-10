using System;
using System.Linq;
using System.Collections.Generic;

namespace XENO.Cards
{
    public abstract class Card
    {
        public readonly int Number;

        public Card(string name, int number, bool canBeGuarded)
        {
            _mame = name;
            Number = number;
            CanBeGuarded = canBeGuarded;
        }

        public virtual void SetRebirth(Card card)
        {
        }

        public virtual Card BeReborn() => null;

        public class InvokeArgments
        {
            public Player Invoker;
            public Player Opponent;
            public List<Card> Deck;
        }

        public void InvokeOn(InvokeArgments args)
        {
            Log.Output($"{ToString()}が使用された.");

            if (CanBeGuarded && args.Opponent.IsGuarding)
            {
                Log.Output("相手が乙女を使用しているため無効化された");
                return;
            }

            BeActivated(args);
        }

        protected readonly bool CanBeGuarded;

        protected virtual void BeActivated(InvokeArgments args)
        {
        }

        public override string ToString()
        {
            return $"{Number}:{_mame}";
        }

        public static List<Card> Deck => new List<Card>
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

        public static void Shuffle(List<Card> cards)
        {
            for(var i = 0; i < cards.Count; i++)
            {
                var target = new Random().Next(0, cards.Count);
                var tmp = cards[i];
                cards[i] = cards[target];
                cards[target] = tmp;
            }
        }

        protected void DoPublicExecutionOn(Player invoker, Player opponent, List<Card> deck, bool byEmperor)
        {
            if(deck.Count == 0)
            {
                return;
            }

            var card = deck[0];
            deck.RemoveAt(0);

            var cards = opponent.DrawAndRevealCards(card);
            var cardNumber = cards.Select(x => x.Number).Distinct().Count() == 1 ? cards[0].Number : byEmperor && cards.Any(x => x is Hero) ? 10 : invoker.SelectOnPublicExecution(opponent);
            Log.Output($"ナンバー:{cardNumber}を指定");
            opponent.Discard(cardNumber, byEmperor);
        }

        private readonly string _mame;
    }
}
