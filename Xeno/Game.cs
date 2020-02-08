using System.Linq;
using System.Collections.Generic;

namespace XENO
{
    using Cards;

    public class Game
    {
        public List<Player> Players;

        public List<Card> Deck;

        public Game(List<Player> players)
        {
            Players = players;
        }

        public List<Player> GetWinners()
        {
            var max = Players.Select(x => x.Power).Max();
            return Players.Where(x => x.Power == max).ToList();
        }

        public Player GetOwner(Card card) => Players.First(x => x.Has(card));

        public Player GetOpponent(Card card) => Players.First(x => x != GetOwner(card));

        public List<Card> RevealedCards => Players.SelectMany(x => x.Trash).ToList();

        public void Start()
        {
            Deck = Card.Deck;
            Card.Shuffle(Deck);

            var rebirthCard = Deck[0];

            Deck.First(x => x is Hero).SetRebirth(rebirthCard);

            Deck.RemoveAt(0);

            foreach (var player in Players)
            {
                player.Recieve(Deck[0]);
                Deck.RemoveAt(0);
            }

            for (var i = 0; Deck.Count > 0 && Players.All(x => x.IsAlive); i = (i + 1) % Players.Count)
            {
                var player = Players[i];
                Log.Output($"プレイヤー:{player.ToString()}のターン 残りデッキ枚数:{Deck.Count}.");
                player.DoAction(this);
            }

            if (Players.All(x => x.Power > 0))
            {
                Log.Output("勝負");
                foreach (var player in Players)
                {
                    Log.Output($"{player.Name} {player.RevealCard()}");
                }
            }

            var winners = GetWinners();
            Log.Output("勝者");
            foreach(var winner in winners)
            {
                Log.Output(winner.Name);
            }
            Log.Output("");
        }
    }
}
