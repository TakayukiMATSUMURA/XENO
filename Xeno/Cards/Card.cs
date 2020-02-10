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
            opponent.DrawAndDiscardByOpponent(deck, invoker, byEmperor);
        }

        private readonly string _mame;

        private readonly bool _canBeGuarded;
    }
}
