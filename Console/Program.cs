using System.Collections.Generic;

namespace XENO.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 100000; i++)
            {
                System.Console.WriteLine($"第{i + 1}ゲーム");
                var players = new List<Player>()
                {
                    new Player("Alice"),
                    new Player("Bob")
                };

                var deck = new Deck();
                deck.Shuffle();
                deck.SetRebirthCard();
                foreach(var player in players)
                {
                    player.Receive(deck.Draw());
                }
                var game = new Game(players, deck);
                game.Start();
            }
        }
    }
}
