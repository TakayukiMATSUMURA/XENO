using System.Linq;
using System.Collections.Generic;

namespace XENO
{
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
            _deck = new Deck();
            _deck.Shuffle();
            _deck.SetRebirthCard();

            foreach (var player in _players)
            {
                player.Recieve(_deck.Draw());
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

        private Deck _deck;
    }
}
