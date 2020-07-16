namespace XENO.Cards
{
    public class Death : Card
    {
        public Death() : base("死神", 5, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            var deck = args.Deck;
            if (deck.Count == 0)
            {
                return;
            }

            var card = deck.Draw();
            var opponent = args.Opponent;
            opponent.Receive(card);
            opponent.DiscardAtRandom();
        }
    }
}
