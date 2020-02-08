namespace XENO.Cards
{
    public class Emperor : Card
    {
        public Emperor() : base("皇帝", 9)
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

            DoPublicExecutionOn(game, true);
        }
    }
}
