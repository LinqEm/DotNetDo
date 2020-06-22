using System.Collections.Generic;

namespace DotNetDo
{
    interface ITaskLocator
    {
        Task? FindByName(string taskName);

        IEnumerable<Task> FindAll();

        IEnumerable<Task> ResolveAll();
    }
}