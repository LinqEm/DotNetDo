using System;
using System.Diagnostics;

namespace Do
{
    /// <summary>
    /// Provides methods for executing shell commands
    /// </summary>
    public static class Executioner
    {
        /// <summary>
        /// Executes the given command in the operating systems shell. 
        /// </summary>
        /// <param name="commands">The command to execute in shell</param>
        public static void Execute(this string commands)
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
                        UseShellExecute = true,
                    }
                };
                process.Start();
                process.WaitForExit();
            }
        }
    }
}
