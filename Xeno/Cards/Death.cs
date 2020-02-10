namespace XENO.Cards
{
    public class Death : Card
    {
        public Death() : base("死神", 5, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            args.Opponent.DrawAndDiscard(args.Deck);
        }
    }
}
