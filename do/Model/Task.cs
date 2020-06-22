using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetDo
{
    internal class Task
    {

        /// <summary>
        /// One or more names for this task. Used to execute the 
        /// task via <c>dotnet do taskName</c>.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// A description for this task. This is shown in <c>dotnet do --list</c>.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// One or more commands to execute in terminal.
        /// </summary>
        public string? Run { get; set; }

        /// <summary>
        /// Working directory relative to the dotnet-tasks.json.
        /// </summary>
        public string? WorkingDirectory { get; set; }

        /// <summary>
        /// Do not restore the terminal to it's original working directory after 
        /// completing this task. Only effective when the <see cref="Task.WorkingDirectory" />
        /// option is given. Default is false.
        /// </summary>
        public bool DoNotRestoreWorkingDirectory { get; set; }

        internal Configuration? ParentConfiguration { get; set; }

        internal IEnumerable<string>? NameArray {
            get => this.Name?.Trim().ToLower().Split(Environment.NewLine).Select(s => s.Trim()).ToList();
            set => this.Name = value != null ? string.Join(Environment.NewLine, value) : null;
        }

        public string? ResolveAbsoluteWorkingDirectory()
        {
            if (this.WorkingDirectory == null)
                return null;
            var workingDirectoryIsRoot = Path.IsPathRooted(this.WorkingDirectory);
            if (workingDirectoryIsRoot)
                if (Directory.Exists(this.WorkingDirectory))
                    return this.WorkingDirectory;
                else
                    return null;
            var configDirPath = Path.GetDirectoryName(this.ParentConfiguration?.FilePath);
            if (configDirPath == null)
                return null;
            var absolutePath = Path.Combine(configDirPath, this.WorkingDirectory);
            if (Directory.Exists(absolutePath))
                return absolutePath;
            return null;
        }

    }
}