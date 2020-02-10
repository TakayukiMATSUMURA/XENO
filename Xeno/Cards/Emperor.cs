namespace XENO.Cards
{
    public class Emperor : Card
    {
        public Emperor() : base("皇帝", 9, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            DoPublicExecutionOn(args.Invoker, args.Opponent, args.Deck, true);
        }
    }
}
