namespace Scoreboard.Core
{
    using System.Collections.Generic;
    using System.Text;
    using Helpers;
    using Interfaces;
    using Wintellect.PowerCollections;

    public class GameDatabase : IGameDatabase
    {
        private readonly Dictionary<string, string> players =
            new Dictionary<string, string>();

        private readonly Dictionary<string, string> games =
            new Dictionary<string, string>();

        private readonly Dictionary<string, SortedDictionary<int, OrderedBag<string>>> scores =
            new Dictionary<string, SortedDictionary<int, OrderedBag<string>>>();

        private readonly Dictionary<string, SortedSet<string>> gamesByPrefix =
            new Dictionary<string, SortedSet<string>>();

        public string RegisterUser(string username, string password)
        {
            if (this.players.ContainsKey(username))
            {
                return "Duplicated user";
            }
            
            this.players.Add(username, password);

            return "User registered";
        }

        public string RegisterGame(string game, string gamePassword)
        {
            if (this.games.ContainsKey(game))
            {
                return "Duplicated game";
            }
            
            // Add to games
            this.games.Add(game, gamePassword);

            // Add to scores
            this.scores.Add(
                game, 
                new SortedDictionary<int, OrderedBag<string>>(
                    Comparer<int>.Create((i, i1) => i1.CompareTo(i))));

            // Add to prefix db
            for (int i = 1; i <= game.Length; i++)
            {
                var prefix = game.Substring(0, i);
                this.gamesByPrefix.AddValueInCollectionToKey(prefix, game);
            }

            return "Game registered";
        }

        public string DeleteGame(string game, string gamePassword)
        {
            if (!this.games.ContainsKey(game) ||
                this.games[game] != gamePassword)
            {
                return "Cannot delete game";
            }

            // Remove from games
            this.games.Remove(game);

            // Remove from scores
            this.scores.Remove(game);

            // Remove from prefix db
            for (int i = 1; i <= game.Length; i++)
            {
                var prefix = game.Substring(0, i);
                this.gamesByPrefix[prefix].Remove(game);
            }

            return "Game deleted";
        }

        public string AddScore(string username, string password, string game, string gamePassword, int score)
        {
            if (!this.games.ContainsKey(game) ||
                !this.players.ContainsKey(username) ||
                this.players[username] != password ||
                this.games[game] != gamePassword)
            {
                return "Cannot add score";
            }

            this.scores[game].AddValueInCollectionToKey(score, username);

            return "Score added";
        }

        public string ShowScoreboard(string game)
        {
            if (!this.games.ContainsKey(game))
            {
                return "Game not found";
            }

            if (this.scores[game].Count == 0)
            {
                return "No score";
            }

            var topScores = new StringBuilder();
            var gameScores = this.scores[game];
            var topScoresIndex = 1;
            foreach (var score in gameScores)
            {
                foreach (var player in score.Value)
                {
                    if (topScoresIndex > 10)
                    {
                        break;
                    }

                    topScores.AppendLine($"#{topScoresIndex} {player} {score.Key}");
                    topScoresIndex++;
                }
            }

            return topScores.ToString().Trim();
        }

        public string ListGamesByPrefix(string gamePrefix)
        {
            if (!this.gamesByPrefix.ContainsKey(gamePrefix) ||
                this.gamesByPrefix[gamePrefix].Count == 0)
            {
                return "No matches";
            }

            var gamesFound = this.gamesByPrefix[gamePrefix];

            var result = new List<string>();
            var index = 1;
            foreach (var game in gamesFound)
            {
                if (index > 10)
                {
                    break;
                }

                result.Add(game);
                index++;
            }

            return string.Join(", ", result);
        }
    }
}