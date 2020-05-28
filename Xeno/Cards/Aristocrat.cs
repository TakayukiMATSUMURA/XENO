using System.Linq;
using System.Collections.Generic;

namespace XENO.Cards
{
    public class Aristocrat : Card
    {
        public Aristocrat(bool isFace = false) : base("貴族", 6, true)
        {
            _isFace = isFace;
        }

        protected override void BeActivated(InvokeArgments args)
        {
            Log.Output($"プレイヤー:{args.Invoker.ToString()}のカードは{args.Invoker.Card}.");
            Log.Output($"プレイヤー:{args.Opponent.ToString()}のカードは{args.Opponent.Card}.");

            if (_isFace)
            {
                var revealedCards = args.Invoker.Trash;
                revealedCards.AddRange(args.Opponent.Trash);
                if (revealedCards.Count(x => x is Aristocrat) == 1)
                {
                    return;
                }
            }

            var players = new List<Player> { args.Invoker, args.Opponent };
            var losers = players.Where(x => x.Power == players.Min(y => y.Power)).ToList();

            foreach (var loser in losers)
            {
                loser.Discard(loser.Power, false);
            }
        }

        private readonly bool _isFace;
    }
}
