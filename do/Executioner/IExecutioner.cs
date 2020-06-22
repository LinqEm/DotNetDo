namespace DotNetDo
{
    public interface IExecutioner
    {

        void Execute(string commands, string? workingDirectory);

    }
}