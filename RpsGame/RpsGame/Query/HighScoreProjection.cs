using System.Collections.Generic;
using System.Linq;
using RpsGame.Events;

namespace RpsGame.Query
{
    public class HighScoreProjection : IEventListener
    {
        private readonly Dictionary<string, int> _highScores = new Dictionary<string, int>();
        public void Receive(IEnumerable<IEvent> events)
        {
            var wins = events.Where(x => x.GetType() == typeof(GameWon));
            foreach (var win in wins)
            {
                UpdateHighScore((GameWon)win);
            }
        }

        private void UpdateHighScore(GameWon win)
        {
            if (!_highScores.ContainsKey(win.Winner))
            {
                _highScores.Add(win.Winner, 0);
            }
            _highScores[win.Winner]++;
        }

        public Dictionary<string, int> HighScores
        {
            get { return _highScores; }
        }
    }
}