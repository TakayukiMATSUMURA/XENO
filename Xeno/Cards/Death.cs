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
            var opponent = game.GetOpponent(this);
            if (opponent.IsGuarding)
            {
                return;
            }

            if (game.Deck.Count > 0)
            {
                var card = game.Deck[0];
                game.Deck.RemoveAt(0);
                opponent.DrawAndDiscard(card);
            }
        }
    }
}
