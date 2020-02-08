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
            var opponent = game.GetOpponent(this);
            if (opponent.IsGuarding)
            {
                return;
            }

            Log.Output($"{opponent.ToString()}のカードは{opponent.RevealCard().ToString()}.");
        }
    }
}
