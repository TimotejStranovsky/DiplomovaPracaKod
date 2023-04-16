using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiScanResults 
{
    public string BSSID;
    public string SSID;
    public int signalLevel;
    public int signalFrequency;

    public WifiScanResults(string pBSSID, string pSSID, string pSignalLevel, string psignalFrequency)
    {
        BSSID = pBSSID;
        SSID = pSSID;
        int.TryParse(pSignalLevel, out signalLevel);
        int.TryParse(psignalFrequency, out signalFrequency);
    }

    public override string ToString()
    {
        return BSSID+" "+SSID+" "+signalLevel+" "+signalFrequency;
    }

    public float GetWifiDistance()
    {
        float exp = (27.55f - (20f * Mathf.Log10(signalFrequency)) + Mathf.Abs(signalLevel)) / 20.0f;
        return Mathf.Pow(10f,exp);
    }
}
