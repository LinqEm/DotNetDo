using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Do {
    class Program {

        private const string CONFIGURATION_FILE_NAME = "dotnet-tasks.yml";
        private static ISerializer _serializer;
        private static IDeserializer _deserializer;

        static Program()
        {
            _serializer = new SerializerBuilder().WithNamingConvention(LowerCaseNamingConvention.Instance).Build();
            _deserializer = new DeserializerBuilder().WithNamingConvention(LowerCaseNamingConvention.Instance).Build();
        }

        /// <summary>
        /// DotNet CLI task runner. 
        /// </summary>
        /// <param name="create">Create a new sample dotnet-tasks.yml</param>
        /// <param name="args">The additional arguments to pass to the task command</param>
        static int Main(bool create, string[] args = null)
        {
            if (create) 
                return Create();
            if (args != null)
            {
                var task = GetTask(args[0]);
                if (task == null)
                {
                    Console.Error.WriteLine($"No task with the name of \"{args[0]}\" was found.");
                    return 7001;
                }
                task.Run.Execute();
            }
            return 0;
        }

        static int Create()
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
                        Run = "echo Hello World!"
                    }
                }
            };
            var yaml = _serializer.Serialize(configuration);
            File.WriteAllText(CONFIGURATION_FILE_NAME, yaml);
            Console.WriteLine("A new tasks file (dotnet-tasks.yml) has been created. Try it out with `dotnet do echo Hello World`.");
            return 0;
        }

        static Task GetTask(string taskName)
        {
            foreach (var possibleConfigPath in FindFiles())
            {
                var contents = File.ReadAllText(possibleConfigPath);
                var config = _deserializer.Deserialize<Configuration>(contents);
                var task = config.Tasks.FirstOrDefault(t => t.Name.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase));
                if (task != null)
                    return task;
            }
            return null;
        }

        static IEnumerable<string> FindFiles()
        {
            string currentDirectory = null;
            do
            {
                currentDirectory = currentDirectory == null ? Directory.GetCurrentDirectory() : Directory.GetParent(currentDirectory).FullName;
                var possibleConfigPath = Path.Combine(currentDirectory, CONFIGURATION_FILE_NAME);
                if (File.Exists(possibleConfigPath))
                    yield return possibleConfigPath;
            } while (Directory.GetDirectoryRoot(currentDirectory) != currentDirectory);
        }

    }
}