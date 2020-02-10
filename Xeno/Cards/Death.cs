namespace XENO.Cards
{
    public class Death : Card
    {
        public Death() : base("死神", 5, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            if (args.Deck.Count == 0)
            {
                return;
            }
            
            var card = args.Deck[0];
            args.Deck.RemoveAt(0);
            args.Opponent.DrawAndDiscard(card);
        }
    }
}
