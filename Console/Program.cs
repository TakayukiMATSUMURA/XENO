using System.Linq;
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
                    new Player("Alice", new Brains.ConsoleInput()),
                    new Player("Bob")
                };
                
                var alice = players[0];
                alice.OnReceive += (card) => Log.Output($"プレイヤー:{alice.ToString()}に{card.ToString()}が配られた.");
                alice.OnAdd += (card) => Log.Output($"プレイヤー:{alice.ToString()}は{card.ToString()}を引いた.");
                alice.OnSage += (cards) => Log.Output($"プレイヤー:{alice.ToString()}は賢者の効果で{string.Join(",", cards.Select(x => x.ToString()).ToArray())}を見た.");
                alice.OnUse += (card) => Log.Output($"プレイヤー:{alice.ToString()}は{card}を使用.");
                alice.OnDiscard += (card) => Log.Output($"プレイヤー:{alice.ToString()}は{card.ToString()}を捨てた.");
                alice.OnRebirth += (card) => Log.Output($"{alice.ToString()}は{card.ToString()}へ転生した.");

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
