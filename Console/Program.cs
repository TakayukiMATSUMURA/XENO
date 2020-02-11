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
                var game = new Game(players);
                game.Start();
            }
        }
    }
}
