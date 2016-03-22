namespace Scoreboard.Core
{
    using System;
    using System.Linq;
    using Interfaces;

    public class Engine : IEngine
    {
        private readonly IGameDatabase gameDatabase;
        private readonly IIOInterface io;

        public Engine(IGameDatabase gameDatabase, IIOInterface io)
        {
            this.gameDatabase = gameDatabase;
            this.io = io;
        }

        public void Run()
        {
            var command = this.io.Read();
            while (command != "End")
            {
                if (!string.IsNullOrEmpty(command))
                {
                    var commandTokens = command.Split();
                    var commandName = commandTokens[0];
                    var commandArgs = commandTokens.Skip(1).ToArray();

                    var commandResult = this.ExecuteCommand(commandName, commandArgs);
                    this.io.Write(commandResult);
                }

                command = this.io.Read();
            }
        }

        private string ExecuteCommand(string commandName, string[] commandArgs)
        {
            switch (commandName)
            {
                case "RegisterUser":
                    return this.gameDatabase.RegisterUser(commandArgs[0], commandArgs[1]);
                case "RegisterGame":
                    return this.gameDatabase.RegisterGame(commandArgs[0], commandArgs[1]);
                case "AddScore":
                    return this.gameDatabase.AddScore(
                        commandArgs[0], 
                        commandArgs[1], 
                        commandArgs[2], 
                        commandArgs[3], 
                        int.Parse(commandArgs[4]));
                case "ShowScoreboard":
                    return this.gameDatabase.ShowScoreboard(commandArgs[0]);
                case "ListGamesByPrefix":
                    return this.gameDatabase.ListGamesByPrefix(commandArgs[0]);
                case "DeleteGame":
                    return this.gameDatabase.DeleteGame(commandArgs[0], commandArgs[1]);
                default:
                    throw new NotImplementedException("Command is not implemented yet");
            }
        }
    }
}