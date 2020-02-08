namespace XENO.Cards
{
    public class Sage : Card
    {
        public Sage() : base("賢者", 7)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            var owner = game.GetOwner(this);
            owner.OnUseSage();
        }
    }
}
