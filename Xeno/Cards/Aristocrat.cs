using System.Linq;
using System.Collections.Generic;

namespace XENO.Cards
{
    public class Aristocrat : Card
    {
        public Aristocrat() : base("貴族", 6, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            Log.Output($"プレイヤー:{args.Invoker.ToString()}のカードは{args.Invoker.RevealCard()}.");
            Log.Output($"プレイヤー:{args.Opponent.ToString()}のカードは{args.Opponent.RevealCard()}.");

            var players = new List<Player> { args.Invoker, args.Opponent };
            var losers = players.Where(x => x.Power == players.Min(y => y.Power)).ToList();

            foreach (var loser in losers)
            {
                loser.Discard(loser.Power, false);
            }
        }
    }
}
