using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnhancedMapMaker
{
    class TP
    {
        [JsonProperty("testingPoints")]
        public List<TestingPoint> TestingPoints { get; set; }

        public override string ToString()
        {
            string result = "";
            foreach (var tp in TestingPoints)
            {
                result += tp.ToString() + "\n";
            }
            return result;
        }
    }
    class TestingPoint
    {
        [JsonProperty("tID")]
        public int id { get; set; }

        [JsonProperty("tX")]
        public int x { get; set; }

        [JsonProperty("tY")]
        public int y { get; set; }

        [JsonProperty("distance")]
        public float distance { get; set; }

        [JsonProperty("BSSID")]
        public string BSSID { get; set; }

        [JsonProperty("RSSI")]
        public int RSSI { get; set; }

        [JsonProperty("zone")]
        public int zone { get; set; }

        public TestingPoint(int id, int x, int y, float distance, string BSSID, int RSSI, int zone)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.distance = distance;
            this.BSSID = BSSID;
            this.RSSI = RSSI;
            this.zone = zone;
        }

        public override string ToString()
        {
            return "" + x + " " + y + " " + distance + " " + BSSID + " " + RSSI + " " + zone;
        }
    }
}
