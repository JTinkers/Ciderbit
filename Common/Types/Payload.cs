using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Types
{
    /// <summary>
    /// Enumeration describing type of data sent through the TCP/IP conduit.
    /// </summary>
    public enum PayloadType
    {
        Code = 1,
        Files = 2
    }

    /// <summary>
    /// Class holding data related to the data payload sent through TCP/IP conduit.
    /// </summary>
    public class Payload
    {
        [JsonProperty]
        public PayloadType Type { get; set; }

        [JsonProperty]
        public byte[] Data { get; set; }

        [JsonProperty]
        public string Info { get; set; }

        [JsonConstructor]
        public Payload(PayloadType type, byte[] data, string info)
        {
            Type = type;
            Data = data;
            Info = info;
        }

        /// <summary>
        /// Returns files stored inside the data payload if it's of a specific type.
        /// </summary>
        /// <returns></returns>
        public string[] GetFiles()
        {
            if (Type != PayloadType.Files)
                return null;

            return Encoding.Default.GetString(Data).Split(';');
        }

        /// <summary>
        /// Turns object into a JSON string variant.
        /// </summary>
        /// <returns>JSON string of a serialized object</returns>
        public string Serialize() => JsonConvert.SerializeObject(this);

        /// <summary>
        /// Rebuilds an object from JSON string.
        /// </summary>
        /// <param name="data">String to use in rebuiliding.</param>
        /// <returns>Rebuilt object.</returns>
        public static Payload Deserialize(byte[] data) => JsonConvert.DeserializeObject<Payload>(Encoding.Default.GetString(data));
    }
}
