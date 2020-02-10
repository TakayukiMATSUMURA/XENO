using System.Collections.Generic;

namespace XENO.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = new int[2];
            for (var i = 0; i < 100000; i++)
            {
                System.Console.WriteLine($"第{i + 1}ゲーム");
                var players = new List<Player>()
                {               
                    new Player("Alice", new Player.Console()),
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
                if(game.GetWinners().Contains(players[0]))
                {
                    counter[0]++;
                }
                if(game.GetWinners().Contains(players[1]))
                {
                    counter[1]++;
                }
                Log.Output($"勝率:{(counter[0] * 100) / (i + 1)}% ({counter[0]}/{i + 1})\n");
            }
        }
    }
}
