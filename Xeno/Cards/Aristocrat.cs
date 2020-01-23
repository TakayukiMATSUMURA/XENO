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
            owner.DuelWith(opponent);
        }
    }
}
