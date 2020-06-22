using System;
using System.Diagnostics;

namespace DotNetDo
{
    /// <summary>
    /// Provides methods for executing shell commands
    /// </summary>
    public class Executioner : IExecutioner
    {
        /// <summary>
        /// Executes the given command in the operating systems shell. 
        /// </summary>
        /// <param name="commands">The command to execute in shell</param>
        /// <param name="workingDirectory">The absolute path for where to execute the command</param>
        public void Execute(string commands, string? workingDirectory)
        {
            foreach (var command in commands.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(command))
                    continue;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("do > ");
                Console.ResetColor();
                Console.WriteLine(command);
                var cmdSplit = command.Split(" ").AsSpan();
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = cmdSplit[0],
                        Arguments = string.Join(' ', cmdSplit.Slice(1).ToArray()),
                        WorkingDirectory = workingDirectory,
                        UseShellExecute = true,
                    }
                };
                process.Start();
                process.WaitForExit();
                Console.WriteLine();
            }
        }
    }
}
