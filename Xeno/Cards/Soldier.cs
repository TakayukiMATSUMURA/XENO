namespace XENO.Cards
{
    public class Soldier : Card
    {
        public Soldier() : base("兵士", 2, true)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            var cardNumber = args.Invoker.SelectOnSoldier();
            Log.Output($"ナンバー:{cardNumber}を指定");
            args.Opponent.Discard(cardNumber, false);  
        }
    }
}
