using System;
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

        public Player GetOwner(Card card) => Players.First(x => x.Cards.Contains(card) || x.LastCard == card);

        public Player GetOpponent(Card card) => Players.First(x => x != GetOwner(card));

        public void Start()
        {
            Deck = Card.Deck;
            Card.Shuffle(Deck);

            _rebirthCard = Deck[0];
            Deck.RemoveAt(0);

            foreach(var player in Players)
            {
                player.Recieve(Deck[0]);
                Deck.RemoveAt(0);
            }

            for (var i = 0; Deck.Count > 0 && Players.All(x => x.Cards.Count == 1); i = (i + 1) % Players.Count)
            {
                var player = Players[i];
                Log.Output($"プレイヤー:{player.ToString()}のターン.");
                player.Draw(Deck);
                player.DoAction(this);
            }

            if (Players.All(x => x.Power > 0))
            {
                Log.Output("勝負");
                foreach (var player in Players)
                {
                    Log.Output(player.ToString());
                }
            }

            var winners = GetWinners();
            Log.Output("勝者");
            foreach(var winner in winners)
            {
                Log.Output(winner.ToString());
            }
            Log.Output("");
        }

        public void Rebirth(Player player)
        {
            Log.Output($"プレイヤー:{player.ToString()}は{_rebirthCard.ToString()}へ転生.");
            player.Rebirth(_rebirthCard);
            _rebirthCard = null;
        }

        private Card _rebirthCard;
    }
}
