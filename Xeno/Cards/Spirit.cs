namespace XENO.Cards
{
    public class Spirit : Card
    {
        public Spirit() : base("精霊", 8, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            Log.Output($"プレイヤー:{args.Invoker.Name}({args.Invoker.Card})とプレイヤー:{args.Opponent.Name}({args.Opponent.Card})はカードを交換した.");
            args.Invoker.ExchangeCardsWith(args.Opponent);
        }
    }
}
