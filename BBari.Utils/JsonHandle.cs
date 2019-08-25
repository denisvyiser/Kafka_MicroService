using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.Utils.JsonExtention
{
    public class JsonHandle
    {
        public JsonHandle(string json)
        {
            Json = json;
        }

        public string Json { get; set; }

        public string JsonKey(string key)
        {
            string value = string.Empty;

            var data = JObject.Parse(this.Json);

            //value = data[$"{key}"].Value<string>();
            //JObject.Parse(line)["ConsumerConfigure"]["GroupId"].Value<string>()

            value = data.SelectToken($"{key}").Value<string>();

            return value;
        }



    }
}
