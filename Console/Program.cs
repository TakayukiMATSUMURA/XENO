using System.Linq;
using System.Collections.Generic;

namespace XENO.Console
{
    class Program
    {
        static void Main(string[] args)
        {
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
