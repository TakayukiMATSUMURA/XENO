namespace XENO.Cards
{
    public class FortuneTeller : Card
    {
        public FortuneTeller() : base("占い師", 3, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            Log.Output($"{args.Opponent.ToString()}のカードは{args.Opponent.RevealCard().ToString()}.");
        }
    }
}
