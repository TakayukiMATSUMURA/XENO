namespace XENO.Cards
{
    public class Spirit : Card
    {
        public Spirit() : base("精霊", 8)
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

            Log.Output($"プレイヤー:{owner.Name}({owner.RevealCard()})とプレイヤー:{opponent.Name}({opponent.RevealCard()})はカードを交換した.");
            owner.ExchangeCardsWith(opponent);
        }
    }
}
