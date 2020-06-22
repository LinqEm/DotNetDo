using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DotNetDo
{
    class ConfigurationsLocator : IConfigurationsLocator
    {


        public IEnumerator<FileInfo> GetEnumerator()
        {
            string currentDirectory = null;
            do
            {
                currentDirectory = currentDirectory == null ? Directory.GetCurrentDirectory() : Directory.GetParent(currentDirectory).FullName;
                var possibleConfigPath = Path.Combine(currentDirectory, Do.CONFIGURATION_FILE_NAME);
                if (File.Exists(possibleConfigPath))
                    yield return new FileInfo(possibleConfigPath);
            } while (Directory.GetDirectoryRoot(currentDirectory) != currentDirectory);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
