namespace XENO.Cards
{
    public class Maiden : Card
    {
        public Maiden() : base("乙女", 4, false)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            args.Invoker.OnUseMaiden();
        }
    }
}
