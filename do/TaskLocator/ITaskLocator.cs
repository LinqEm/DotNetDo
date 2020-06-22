namespace DotNetDo
{
    interface ITaskLocator
    {
        Task Find(string taskName);
    }
}