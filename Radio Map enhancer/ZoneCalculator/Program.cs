using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZoneCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            JObject obj = JObject.Parse(File.ReadAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\databaseZone.json"));
            JObject obj2 = JObject.Parse(File.ReadAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\databaseTP.json"));
            //JObject obj3 = JObject.Parse(File.ReadAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\databaseAP.json"));
            var zones = JsonConvert.DeserializeObject<ZoneRoot>(obj.ToString());
            var tps = JsonConvert.DeserializeObject<TP>(obj2.ToString());
            //var aps = JsonConvert.DeserializeObject<AP>(obj3.ToString());

            //Console.WriteLine(tps.ToString());
            //Console.WriteLine("-------------------------------------------");

            foreach (var zone in zones.zones)
            {
                var zoneTPs = tps.TestingPoints.FindAll(elm => elm.zone == zone.id);
                if (zoneTPs.Count > 0)
                {
                    var constants = new ConstantCalculator();
                    //List<AccessPoint> zoneAP = new List<AccessPoint>();
                    foreach (var tp in zoneTPs)
                    {
                        constants.IncreaseConstants(tp.distance, tp.RSSI);
                        //Console.WriteLine(constants.ToString());
                    }
                    constants.FinalizeConstants();
                    //Console.WriteLine(constants.ToString());
                    zone.b1 = constants.CalculateB1();
                    zone.b0 = constants.CalculateB0(zone.b1);
                }
                else
                {
                    zone.b1 = 0;
                    zone.b0 = 0;
                }
            }

            //Console.WriteLine(tps.ToString());
            //foreach(var ap in zoneAP)
            //    Console.WriteLine(ap.ToString());
            JObject exp = (JObject)JToken.FromObject(zones);
            File.WriteAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\ZoneExport.json", exp.ToString());
        }
    }
}
