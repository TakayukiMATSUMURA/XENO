namespace XENO.Cards
{
    public class Death : Card
    {
        public Death() : base("死神", 5)
        {
        }


        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            if (game.Deck.Count > 0)
            {
                var opponent = game.GetOpponent(this);
                var card = game.Deck[0];
                game.Deck.RemoveAt(0);
                Log.Output($"プレイヤー:{opponent.ToString()}は{ToString()}の効果で{card.ToString()}を引いた.");
                opponent.Cards.Add(card);

                var target = opponent.Cards[new System.Random().Next(0, opponent.Cards.Count)];
                Log.Output($"プレイヤー:{opponent.ToString()}は{target.ToString()}を捨てた.");
                opponent.Discard(target);
            }
        }
    }
}
