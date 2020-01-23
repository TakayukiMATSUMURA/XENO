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

            var trash = game.Players.SelectMany(x => x.Trash);
            if (trash.Count(x => x is Boy) == 2)
            {
                DoPublicExecutionOn(game, false);
            }
        }    
    }
}
