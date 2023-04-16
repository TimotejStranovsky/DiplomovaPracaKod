using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    [SerializeField] TextAsset JSONFile;
    public TPList listOfTestPoints = new TPList();
    public ZoneList listOfZones = new ZoneList();
    public CZList listOfConnectedZones = new CZList();

    //This class is used to load individual entries in the JSON file
    [System.Serializable]
    public class TestingPoints
    {
        public int tID;
        public int tX;
        public int tY;
        public string BSSID;
        public string RSSI;

        public override string ToString()
        {
            return "" + tID + "_" + tX + "_" + tY + "_" + BSSID + "_" + RSSI + "~";
        }
    }

    //This class stores every entry from the JSON file containing all of the card combinations
    [System.Serializable]
    public class TPList
    {
        public TestingPoints[] testingPoints;
        public string GetFilteredTP(string[] filter)
        {
            List<TestingPoints> filteredTP = new List<TestingPoints>();
            foreach(var element in filter)
            {
                foreach (var cz in testingPoints)
                {
                    if (cz.BSSID == element)
                    {
                        filteredTP.Add(cz);
                    }
                }
            }
            return PrintConnectedZones(filteredTP.ToArray());
        }

        public string PrintConnectedZones(TestingPoints[] TPS)
        {
            string returnText = "";
            foreach (var tp in TPS)
            {
                returnText += tp.ToString();
            }
            return returnText;
        }
        public override string ToString()
        {
            return PrintConnectedZones(testingPoints);
        }
    }

    //This class is used to load individual entries in the JSON file
    [System.Serializable]
    public class Zones
    {
        public int zID;
        public string name;
        public int zX;
        public int zY;
        public float width;
        public float height;

        public override string ToString()
        {
            return ""+ zID+ "_" + name + "_" + zX + "_" + zY + "_" + width + "_" + height;
        }
    }

    //This class stores every entry from the JSON file containing all of the card combinations
    [System.Serializable]
    public class ZoneList
    {
        public Zones[] zones;
        public string GetFilteredZones(int filter)
        {
            List<Zones> filteredZones = new List<Zones>();
            foreach (var zone in zones)
            {
                if (zone.zID == filter)
                {
                    filteredZones.Add(zone);
                }
            }
            return PrintConnectedZones(filteredZones.ToArray());
        }

        public string PrintConnectedZones(Zones[] conZons)
        {
            string returnText = "";
            foreach (var zone in conZons)
            {
                returnText += zone.ToString();
            }
            return returnText;
        }
        public override string ToString()
        {
            return PrintConnectedZones(zones);
        }
    }
    //This class is used to load individual entries in the JSON file
    [System.Serializable]
    public class ConnectedZones
    {
        public int zOrgID;
        public string name;
        public string zID;
        public int zX;
        public int zY;
        public float width;
        public float height;
        public string cardDir;
        public int cardDirOrd;

        public override string ToString()
        {
            return ""+ zID + "_" + name + "_" + zX + "_" + zY + "_" + width + "_" + height +"_"+ cardDir + "_" + cardDirOrd+"~";
        }
    }

    //This class stores every entry from the JSON file containing all of the card combinations
    [System.Serializable]
    public class CZList
    {
        public ConnectedZones[] connectedZones;

        public string GetFilteredCZ(int filter)
        {
            List<ConnectedZones> filteredConZones = new List<ConnectedZones>();
            foreach(var cz in connectedZones)
            {
                if(cz.zOrgID == filter)
                {
                    filteredConZones.Add(cz);
                }
            }
            return PrintConnectedZones(filteredConZones.ToArray());
        }

        public string PrintConnectedZones(ConnectedZones[] conZons)
        {
            string returnText = "";
            foreach (var cz  in conZons)
            {
                returnText += cz.ToString();
            }
            return returnText;
        }

        public override string ToString()
        {
            return PrintConnectedZones(connectedZones);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        listOfTestPoints = JsonUtility.FromJson<TPList>(JSONFile.text);
        listOfZones = JsonUtility.FromJson<ZoneList>(JSONFile.text);
        listOfConnectedZones = JsonUtility.FromJson<CZList>(JSONFile.text);
        //Debug.Log(listOfTestPoints.ToString());
        //Debug.Log(listOfTestPoints.GetFilteredTP());
        //Debug.Log(listOfConnectedZones.GetFilteredCZ(1));

    }

    public string GetZones(int filter)
    {
        return listOfZones.GetFilteredZones(filter);
    }
    public string GetTP(string[] filter)
    {
        return listOfTestPoints.GetFilteredTP(filter);
    }
    public string GetConnectedZones(int filter)
    {
        return listOfConnectedZones.GetFilteredCZ(filter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
