using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Types
{
    /// <summary>
    /// Class responsible for holding data kept inside the package.info file.
    /// </summary>
    public class ScriptInfo
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Author { get; set; }

        [JsonProperty]
        public string Version { get; set; }

        [JsonProperty]
        public string Contact { get; set; }

        [JsonProperty]
        public string Website { get; set; }

        [JsonProperty]
        public string EntryClass { get; set; }

        [JsonProperty]
        public string EntryMethod { get; set; }

        [JsonProperty]
        public string LinkedAssemblies { get; set; } = "System.dll;CiderbitCommon.dll;CiderbitComponent.dll";

        /// <summary>
        /// Turns object into a JSON string variant.
        /// </summary>
        /// <returns>JSON string of the serialized object.</returns>
        public string Serialize() => JsonConvert.SerializeObject(this);

        /// <summary>
        /// Recreates an object from JSON string.
        /// </summary>
        /// <param name="data">String to use for recreation.</param>
        /// <returns>Recreated object from the JSON string.</returns>
        public static ScriptInfo Deserialize(string data) => JsonConvert.DeserializeObject<ScriptInfo>(data);
    }
}
