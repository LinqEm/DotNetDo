using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DotNetDo
{
    class Do 
    {

        internal const string CONFIGURATION_FILE_NAME = "dotnet-tasks.yml";

        private IExecutioner _executioner;
        private ISerializer _serializer;
        private ITaskLocator _taskLocator;

        Do(ISerializer serializer, IExecutioner executioner, ITaskLocator taskLocator)
        {
            this._serializer = serializer;
            this._executioner = executioner;
            this._taskLocator = taskLocator;
        }

        /// <summary>
        /// Run the application - responding to the given
        /// command line arguments appropriately. 
        /// </summary>
        /// <param name="command">The initial command if any</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public int TheDo(string? command, string[]? args)
        {
            if (command == "--create")
                return Create();
            if (command == "--version")
                return Version();
            else if (command == "--help" || command == "-h" || command == "-?")
                return Help();
            else if (command != null)
            {
                var task = this._taskLocator.Find(command);
                if (task == null)
                {
                    Console.Error.WriteLine($"No task with the name of \"{args[0]}\" was found.");
                    return 7001;
                }
                if (string.IsNullOrWhiteSpace(task.Run))
                    return 0;
                this._executioner.Execute(task.Run, task.ResolveAbsoluteWorkingDirectory());
            } else
                return Help();
            return 0;
        }

        private int Create()
        {
            if (File.Exists(CONFIGURATION_FILE_NAME))
            {
                Console.Error.WriteLine("A tasks file (dotnet-tasks.yml) already exists here. Did you intend to create a new tasks file in a different directory?");
                return 7002;
            }
            var configuration = new Configuration
            {
                Tasks = new List<Task> {
                    new Task {
                        Name = "echo",
                        Description = "A sample task that simply echos \"Hello World\"",
                        Run = "echo Hello World!"
                    }
                }
            };
            var yaml = _serializer.Serialize(configuration);
            File.WriteAllText(CONFIGURATION_FILE_NAME, yaml);
            Console.WriteLine("A new tasks file (dotnet-tasks.yml) has been created. Try it out with `dotnet do echo`.");
            return 0;
        }

        private int Help()
        {
            Console.WriteLine();
            Console.WriteLine("DotNet Do [The Do]");
            Console.WriteLine("  The DotNet CLI task runner. ");
            Console.WriteLine("  " + Assembly.GetEntryAssembly()?.GetName().Version);
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  dotnet do <task> [<args>...]");
            Console.WriteLine("  dotnet do <option>");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --list\t\tList all available tasks");
            Console.WriteLine("  --create\t\tCreate a new dotnet-tasks.yml");
            Console.WriteLine("  --version\t\tShow version information");
            Console.WriteLine("  -?, -h, --help\tShow help and usage information");
            Console.WriteLine();
            return 0;
        }

        private int Version()
        {
            Console.WriteLine(Assembly.GetEntryAssembly()?.GetName().Version);
            return 0;
        }

        /// <summary>
        /// DotNet CLI task runner. 
        /// </summary>
        /// <param name="args"></param>
        static int Main(string[] args)
        {
            var serializer = new SerializerBuilder().ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults).WithNamingConvention(LowerCaseNamingConvention.Instance).Build();
            var deserializer = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
            var @do = new Do(serializer, new Executioner(), new TaskLocator(new ConfigurationsLoader(new ConfigurationsLocator(), deserializer)));
            return @do.TheDo(args.FirstOrDefault(), args.Length > 1 ? args.AsSpan().Slice(1).ToArray() : null);
        }

    }
}