namespace XENO.Cards
{
    public class Maiden : Card
    {
        public Maiden() : base("乙女", 4)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            var owner = game.GetOwner(this);
            owner.OnUseMaiden();
        }
    }
}
