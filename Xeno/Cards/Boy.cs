using System.Linq;

namespace XENO.Cards
{
    public class Boy : Card
    {
        public Boy() : base("少年", 1, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            var revealedCards = args.Invoker.Trash;
            revealedCards.AddRange(args.Opponent.Trash);
            if (revealedCards.Count(x => x is Boy) == 2)
            {
                DoPublicExecutionOn(args.Invoker, args.Opponent, args.Deck, false);
            }
        }    
    }
}
