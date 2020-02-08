using System.Linq;

namespace XENO.Cards
{
    public class Aristocrat : Card
    {
        public Aristocrat() : base("貴族", 6)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            var owner = game.GetOwner(this);
            var opponent = game.GetOpponent(this);
            if(opponent.IsGuarding)
            {
                return;
            }

            foreach(var player in game.Players)
            {
                Log.Output($"プレイヤー:{player.ToString()}のカードは{player.RevealCard()}.");
            }

            var losers = game.Players.Where(x => x.Power == game.Players.Min(x => x.Power)).ToList();
            foreach (var loser in losers)
            {
                loser.Discard(loser.Power, false);
            }
        }
    }
}
