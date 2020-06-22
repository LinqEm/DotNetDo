using System;
using System.Collections.Generic;
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

        public Task? FindByName(string taskName)
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

        public IEnumerable<Task> FindAll()
        {
            foreach (var config in this._configsLoader)
                foreach (var task in config.Tasks)
                    if (!string.IsNullOrWhiteSpace(task.Name))
                        yield return task;
        }

        public IEnumerable<Task> ResolveAll()
        {
            var taskNamesVisited = new List<string>();
            foreach (var task in this.FindAll())
            {
                var taskNames = task.Name!.Trim().Split(Environment.NewLine).Select(t => t.Trim().ToLower()).Where(t => !taskNamesVisited.Contains(t)).ToList();
                if (!taskNames.Any()) 
                    continue;
                taskNamesVisited.AddRange(taskNames);
                task.Name = string.Join(Environment.NewLine, taskNames);
                yield return task;
            }
        }


    }
}
