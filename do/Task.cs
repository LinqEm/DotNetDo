using System;
using System.Collections.Generic;

namespace Do {
    internal class Task
    {

        /// <summary>
        /// One or more names for this task. Used to execute the 
        /// task via <c>dotnet do taskName</c>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// One or more commands to execute in terminal.
        /// </summary>
        public string Run { get; set; }

        /// <summary>
        /// Working directory relative to the dotnet-tasks.json.
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Do not restore the terminal to it's original working directory after 
        /// completing this task. Only effective when the <see cref="Task.WorkingDirectory" />
        /// option is given.
        /// </summary>
        public bool DoNotRestore { get; set; }

    }
}