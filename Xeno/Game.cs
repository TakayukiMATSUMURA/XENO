using System.Linq;
using System.Collections.Generic;

namespace XENO
{
    using Cards;

    public class Game
    {
        public Game(List<Player> players)
        {
            _players = players;
        }

        public List<Player> GetWinners()
        {
            var max = _players.Select(x => x.Power).Max();
            return _players.Where(x => x.Power == max).ToList();
        }

        public void Start()
        {
            _deck = Card.Deck;
            Card.Shuffle(_deck);

            var rebirthCard = _deck[0];
            _deck.First(x => x is Hero).SetRebirth(rebirthCard);

            _deck.RemoveAt(0);

            foreach (var player in _players)
            {
                player.Recieve(_deck[0]);
                _deck.RemoveAt(0);
            }

            for (var i = 0; _deck.Count > 0 && _players.All(x => x.IsAlive); i = (i + 1) % _players.Count)
            {
                var player = _players[i];
                Log.Output($"プレイヤー:{player.ToString()}のターン 残りデッキ枚数:{_deck.Count}.");
                player.DoAction(_deck, _players.First(x => x != player));
            }

            if (_players.All(x => x.IsAlive))
            {
                Log.Output("勝負");
                foreach (var player in _players)
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

        private List<Player> _players;

        private List<Card> _deck;
    }
}
