﻿namespace XENO.Cards
{
    public class Soldier : Card
    {
        public Soldier() : base("兵士", 2)
        {
        }

        public override void InvokeOn(Game game)
        {
            base.InvokeOn(game);

            var opponent = game.GetOpponent(this);
            var cardNumber = new System.Random().Next(1, 10 + 1);
            //var cardNumber = opponent.Cards[0].Number; // 確実に仕留める
            Log.Output($"ナンバー:{cardNumber}を指定");
            opponent.Discard(cardNumber);

            // 転生
            if(cardNumber == 10 && opponent.Cards.Count == 0)
            {
                game.Rebirth(opponent);
            }            
        }
    }
}