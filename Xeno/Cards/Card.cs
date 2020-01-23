using System;
using System.Linq;
using System.Collections.Generic;

namespace XENO.Cards
{
    public abstract class Card
    {
        public readonly string Name;

        public readonly int Number;

        public Card(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public virtual void SetRebirth(Card card)
        {
        }

        public virtual Card BeReborn() => null;

        public virtual void InvokeOn(Game game)
        {
            Log.Output($"{ToString()}が使用された.");
        }

        public override string ToString()
        {
            return $"{Number}:{Name}";
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

        protected void DoPublicExecutionOn(Game game, bool byEmperor)
        {
            if(game.Deck.Count > 0)
            {
                var owner = game.GetOwner(this);
                var opponent = game.GetOpponent(this);
                var card = game.Deck[0];
                game.Deck.RemoveAt(0);
                var cards = opponent.DrawAndRevealCards(card);

                var cardNumber = byEmperor && cards.Any(x => x is Hero) ? 10 : owner.SelectOnPublicExecution(game);
                Log.Output($"ナンバー:{cardNumber}を指定");
                opponent.Discard(cardNumber, byEmperor);
            }
        }
    }
}
