using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnhancedMapMaker
{
    public class ZoneRoot
    {
        [JsonProperty("zones")]
        public List<Zone> zones { get; set; }

        public override string ToString()
        {
            string result = "";
            foreach (var zone in zones)
            {
                result += zone.ToString() + "\n";
            }
            return result;
        }
    }

    public class Zone
    {
        [JsonProperty("zID")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("zX")]
        public int x { get; set; }

        [JsonProperty("zY")]
        public int y { get; set; }

        [JsonProperty("width")]
        public int width { get; set; }

        [JsonProperty("height")]
        public int height { get; set; }

        [JsonProperty("b1")]
        public float b1 { get; set; }

        [JsonProperty("b0")]
        public float b0 { get; set; }

        public override string ToString()
        {
            return "" + name + " " + x + " " + y + " " + width + " " + height + " " + b0 + " " + b1;
        }
    }
}
