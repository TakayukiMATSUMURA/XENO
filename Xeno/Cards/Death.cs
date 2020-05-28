namespace XENO.Cards
{
    public class Death : Card
    {
        public Death() : base("死神", 5, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            var invoker = args.Invoker;
            var deck = args.Deck;
            if (deck.Count == 0)
            {
                return;
            }

            var card = deck.Draw();
            invoker.Recieve(card);
            args.Invoker.DiscardAtRandom();
        }
    }
}
