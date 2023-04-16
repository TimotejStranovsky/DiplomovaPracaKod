using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EnhancedMapMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            JObject obj = JObject.Parse(File.ReadAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\ZoneExport.json"));
            JObject obj2 = JObject.Parse(File.ReadAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\databaseTP.json"));
            JObject obj3 = JObject.Parse(File.ReadAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\databaseAP.json"));
            var zones = JsonConvert.DeserializeObject<ZoneRoot>(obj.ToString());
            var tps = JsonConvert.DeserializeObject<TP>(obj2.ToString());
            var aps = JsonConvert.DeserializeObject<AP>(obj3.ToString());

            //Console.WriteLine(tps.ToString());
            //Console.WriteLine("-------------------------------------------");

            foreach (var zone in zones.zones)
            {
                var zoneTPs = tps.TestingPoints.FindAll(elm => elm.zone == zone.id);
                if (zoneTPs.Count > 0)
                {
                    List<AccessPoint> zoneAP = new List<AccessPoint>();
                    foreach (var tp in zoneTPs)
                    {
                        var foundAPs = aps.AccessPoints.FindAll(elm => elm.BSSID == tp.BSSID);
                        foreach(var found in foundAPs)
                        {
                            if(!zoneAP.Any(p=>found.BSSID==p.BSSID))
                                zoneAP.Add(found);
                        }
                    }
                    for (int y = zone.y; y < zone.height; y++)
                    {
                        for (int x = zone.x; x < zone.width; x++)
                        {
                           // Console.WriteLine("X: "+x +" Y : "+y);
                            var existingTP = zoneTPs.FindAll(elm => elm.x == x && elm.y == y);
                            if (existingTP.Count > 0)
                            {
                                break;

                            }
                            foreach (var ap in zoneAP)
                            {
                                float newDistance = (float)Math.Sqrt(Math.Pow(ap.x - x, 2) + Math.Pow(ap.y - y, 2));
                                int newRSSI = (int)(zone.b1 * newDistance + zone.b0);
                                tps.TestingPoints.Add(new TestingPoint(-1,x,y, newDistance, ap.BSSID,newRSSI,zone.id));
                            }
                        }

                    }
                }
            }

            //Console.WriteLine(tps.ToString());
            //foreach(var ap in zoneAP)
            //    Console.WriteLine(ap.ToString());
            JObject exp = (JObject)JToken.FromObject(tps);
            File.WriteAllText(@"c:\Users\Admin\Desktop\Diplomovka_kod\Enhanced Map\JSONs\TPExport.json",exp.ToString());
        }

    }
}
