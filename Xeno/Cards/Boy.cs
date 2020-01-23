using System.Linq;

namespace XENO.Cards
{
    public class Boy : Card
    {
        public Boy() : base("少年", 1)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            if (game.RevealedCards.Count(x => x is Boy) == 2)
            {
                DoPublicExecutionOn(game, false);
            }
        }    
    }
}
