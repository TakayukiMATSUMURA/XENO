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

            DoPublicExecutionOn(game, true);
        }
    }
}
