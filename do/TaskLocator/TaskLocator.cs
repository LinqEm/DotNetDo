using System;
using System.Linq;

namespace DotNetDo
{
    class TaskLocator : ITaskLocator
    {
        private readonly IConfigurationsLoader _configsLoader;

        public TaskLocator(IConfigurationsLoader configsLoader)
        {
            this._configsLoader = configsLoader;
        }

        public Task? Find(string taskName)
        {
            foreach (var config in this._configsLoader)
            {
                foreach (var task in config.Tasks)
                {
                    var taskAliases = task?.Name?.Trim().Split(Environment.NewLine);
                    if (taskAliases?.Any(n => n.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase)) ?? false)
                        return task;
                }
            }
            return null;
        }

    }
}
