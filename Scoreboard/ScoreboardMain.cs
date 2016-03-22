namespace Scoreboard
{
    using Core;
    using Interfaces;
    using IO;

    public static class ScoreboardMain
    {
        public static void Main()
        {
            IGameDatabase gameDatabase = new GameDatabase();
            IIOInterface io = new ConsoleInterface();

            IEngine engine = new Engine(gameDatabase, io);

            engine.Run();
        }
    }
}
