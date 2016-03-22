namespace Scoreboard.Interfaces
{
    public interface IIOInterface
    {
        void Write(string text, params object[] @params);

        string Read();
    }
}