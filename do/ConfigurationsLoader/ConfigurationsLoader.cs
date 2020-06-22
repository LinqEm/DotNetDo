using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace DotNetDo
{
    class ConfigurationsLoader : IConfigurationsLoader
    {
        
        private readonly IConfigurationsLocator _locator;
        private readonly IDeserializer _deserializer;

        public ConfigurationsLoader(IConfigurationsLocator locator, IDeserializer deserializer)
        {
            this._locator = locator;
            this._deserializer = deserializer;
        }

        public IEnumerator<Configuration> GetEnumerator()
        {
            foreach (var file in this._locator)
            {
                var contents = File.ReadAllText(file.FullName);
                var config = this._deserializer.Deserialize<Configuration>(contents);
                config.Tasks?.ForEach(t => t.ParentConfiguration = config);
                config.FilePath = file.FullName;
                yield return config;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
