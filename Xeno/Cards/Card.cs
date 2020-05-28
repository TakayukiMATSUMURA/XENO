using System.Linq;

namespace XENO.Cards
{
    public abstract class Card
    {
        public readonly int Number;

        public Card(string name, int number, bool canBeGuarded)
        {
            _mame = name;
            Number = number;
            _canBeGuarded = canBeGuarded;
        }

        public virtual void SetRebirth(Card card)
        {
        }

        public virtual Card BeReborn() => null;

        public class InvokeArgments
        {
            public Player Invoker;
            public Player Opponent;
            public Deck Deck;
        }

        public void InvokeOn(InvokeArgments args)
        {
            Log.Output($"{ToString()}が使用された.");

            if (_canBeGuarded && args.Opponent.IsGuarding)
            {
                Log.Output("相手が乙女を使用しているため無効化された");
                return;
            }

            BeActivated(args);
        }

        protected virtual void BeActivated(InvokeArgments args)
        {
        }

        public override string ToString()
        {
            return $"{Number}:{_mame}";
        }

        protected void DoPublicExecutionOn(Player invoker, Player opponent, Deck deck, bool byEmperor)
        {
            if (deck.Count == 0)
            {
                return;
            }

            var card = deck.Draw();
            opponent.Receive(card);
            var cards = opponent.Cards;
            var cardNumber = cards.Select(x => x.Number).Distinct().Count() == 1 ? cards[0].Number : byEmperor && cards.Any(x => x is Hero) ? 10 : invoker.SelectOnPublicExecution(opponent);
            opponent.Discard(cardNumber, byEmperor);
        }

        private readonly string _mame;

        private readonly bool _canBeGuarded;
    }
}
