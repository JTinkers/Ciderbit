using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Types
{
    public enum PayloadType
    {
        Code = 1,
        Files = 2
    }

    public class Payload
    {
        [JsonProperty]
        public PayloadType Type { get; set; }

        [JsonProperty]
        public byte[] Data { get; set; }

        [JsonConstructor]
        public Payload(PayloadType type, byte[] data)
        {
            Type = type;
            Data = data;
        }

        public string[] GetFiles()
        {
            if (Type != PayloadType.Files)
                return null;

            return Encoding.Default.GetString(Data).Split(';');
        }

        public string Serialize() => JsonConvert.SerializeObject(this);

        public static Payload Deserialize(byte[] data) => JsonConvert.DeserializeObject<Payload>(Encoding.Default.GetString(data));
    }
}
