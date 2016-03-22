namespace Scoreboard.Interfaces
{
    public interface IGameDatabase
    {
        string RegisterUser(string username, string password);

        string RegisterGame(string game, string gamePassword);

        string AddScore(string username, string password, string game, string gamePassword, int score);

        string ShowScoreboard(string game);

        string ListGamesByPrefix(string gamePrefix);

        string DeleteGame(string game, string gamePassword);
    }
}