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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var command in commands.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(command))
                    continue;
                WriteDo("do > ");
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
            stopwatch.Stop();
            string? timingOutput = null;
            if (stopwatch.Elapsed.TotalMilliseconds > 1000)
                timingOutput = stopwatch.Elapsed.TotalSeconds.ToString("F3") + "s";
            else
                timingOutput = stopwatch.Elapsed.TotalMilliseconds.ToString("F0") + "ms";
            WriteDo($"Did the do in {timingOutput}.");
            Console.WriteLine();
        }

        private void WriteDo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(message);
            Console.ResetColor();

        }
    }
}
