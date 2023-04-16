using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnhancedMapMaker
{
    class AP
    {
        [JsonProperty("accessPoints")]
        public List<AccessPoint> AccessPoints { get; set; }

        public override string ToString()
        {
            string result = "";
            foreach(var ap in AccessPoints)
            {
                result += ap.ToString() + "\n";
            }
            return result;
        }
    }
    class AccessPoint
    {
        [JsonProperty("aID")]
        public int id { get; set; }

        [JsonProperty("aX")]
        public int x { get; set; }

        [JsonProperty("aY")]
        public int y { get; set; }

        [JsonProperty("BSSID")]
        public string BSSID { get; set; }

        public override string ToString()
        {
            return "" + x + " " + y + " " + BSSID;
        }
    }
}
