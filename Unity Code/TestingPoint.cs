using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPoint
{
    public string id;
    public int x;
    public int y;
    public List<AccessPoint> accessPoints;
    public float distance;

    public TestingPoint(string pId, string pX, string pY)
    {
        id = pId;
        int.TryParse(pX, out x);
        int.TryParse(pY, out y);
        accessPoints = new List<AccessPoint>();
    }

    public void CalculateDistance(List<WifiScanResults> listOfNetworks)
    {
        float d = 0;
        foreach (var network in listOfNetworks)
        {
            foreach(var point in accessPoints)
            {
                if (point.BSSID == network.BSSID)
                {
                    d += Mathf.Pow((point.RSSI - network.signalLevel),2);
                    //Debug.Log(id + " " + point.RSSI + " " + network.signalLevel + " " + Mathf.Pow((point.RSSI - network.signalLevel), 2));
                }
            }
        }
        distance = Mathf.Sqrt(d);
        //Debug.Log(id + " " + distance);
    }
}

public class AccessPoint
{
    public string BSSID;
    public int RSSI;

    public AccessPoint(string pBSSID, string pRSSI)
    {
        BSSID = pBSSID;
        int.TryParse(pRSSI, out RSSI);
    }
}

public class ConnectedZone
{
    Zone zone;
    string cardinalDirection;
    int cdOrder;

    public Zone Zone=>zone;
    public string CardinalDirection=> cardinalDirection;
    public int CdOrder=> cdOrder;

    public ConnectedZone(Zone pZone, string pCD, string pCDO)
    {
        zone = pZone;
        cardinalDirection = pCD;
        int.TryParse(pCDO, out cdOrder);
    }

    public override string ToString()
    {
        return zone.ToString()+" Direction: "+cardinalDirection+" Order: "+cdOrder;
    }
}

public class Zone
{
    int id;
    string name;
    float x;
    float y;
    float width;
    float height;

    public int ID => id;
    public string Name => name;
    public float X => x;
    public float Y => y;
    public float Width => width;
    public float Height => height;

    public Zone(string pID, string pName, string pX, string pY, string pWidth, string pHeight)
    {
        int.TryParse(pID, out id);
        name = pName;
        float.TryParse(pX, out x);
        float.TryParse(pY, out y);
        float.TryParse(pWidth, out width);
        float.TryParse(pHeight, out height);
    }

    public override string ToString()
    {
        return name + " Id: " + id + " X: " + x + " Y: " + y + " Width: " + width + " Height: " + height;
    }
}