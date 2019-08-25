using BBari.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.Utils.JsonExtention
{
    public class JsonAdapter<T>
    {
        public static string Serialize(T message) => JsonConvert.SerializeObject(message);

        public static T Deserialize(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
