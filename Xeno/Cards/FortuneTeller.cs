namespace XENO.Cards
{
    public class FortuneTeller : Card
    {
        public FortuneTeller() : base("占い師", 3)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            var target = game.GetOpponent(this);
            Log.Output($"{target.ToString()}のカードは{target.Cards[0].ToString()}.");
        }
    }
}
