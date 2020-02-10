namespace XENO.Cards
{
    public class Sage : Card
    {
        public Sage() : base("賢者", 7, false)
        {
        }

        protected override void BeActivated(InvokeArgments args)
        {
            args.Invoker.OnUseSage();
        }
    }
}
