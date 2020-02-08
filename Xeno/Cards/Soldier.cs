namespace XENO.Cards
{
    public class Soldier : Card
    {
        public Soldier() : base("兵士", 2)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            var owner = game.GetOwner(this);
            var opponent = game.GetOpponent(this);
            if (opponent.IsGuarding)
            {
                return;
            }

            var cardNumber = owner.SelectOnSoldier();
            Log.Output($"ナンバー:{cardNumber}を指定");
            opponent.Discard(cardNumber, false);  
        }
    }
}
