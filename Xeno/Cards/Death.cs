namespace XENO.Cards
{
    public class Death : Card
    {
        public Death() : base("死神", 5)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            if (game.Deck.Count > 0)
            {
                var opponent = game.GetOpponent(this);
                var card = game.Deck[0];
                game.Deck.RemoveAt(0);
                opponent.DrawAndDiscard(card);
            }
        }
    }
}
