using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;


public class WifiScanner : MonoBehaviour
{
    public Button showToastButton;
    public TMP_Text text;
    public TMP_Text timer;
    public TMP_Text faketoast;
    public float timeBetweenScans = 30f;
    public PlayerController pc;
    //public ToastMaker toastMaker;

    AndroidJavaObject unityActivity;
    AndroidJavaObject scannerObject;
    List<WifiScanResults> listOfNetworks;
    bool wifiManagerActive = false;
    float scanTimer;

    bool success_test;
    bool timerOn;


    void Start()
    {
        showToastButton.onClick.AddListener(OnShowToastClicked);
        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        if(faketoast!=null)
            faketoast.text = "start";
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        if (faketoast != null)
            faketoast.text = "before toast";
        //toastMaker.CallToast("AAAAAAAAH", false);
        faketoast.text = "AAAAAAAAH";

        string[] permissions = new string[3];
        permissions[0] = Permission.CoarseLocation;
        permissions[1] = Permission.FineLocation;
        Permission.RequestUserPermissions(permissions);
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) && Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            text.text = "HERE";
            //toastMaker.CallToast("we got em boiz", false);
            if (faketoast != null)
                faketoast.text = "we got em boiz";
            wifiManagerActive = GetWifiManager();
        }
        else
        {
            //toastMaker.CallToast("fuck", false);
            faketoast.text = "";
            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                if (faketoast != null)
                    faketoast.text += "fine loc";
                //toastMaker.CallToast("we have fine location", false);

            }
            if (Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
            {
                if (faketoast != null)
                    faketoast.text += "coarse loc";
                //toastMaker.CallToast("we have coarse location", false);

            }
        }
        scanTimer = timeBetweenScans;
        timerOn = false;
    }


    public void OnShowToastClicked()
    {
#if UNITY_ANDROID
        //toastMaker.CallToast("boop",false);
        //ScanForNetworks();
        TurnOnTimer(!timerOn);
#else
		Debug.Log("No Toast setup for this platform.");
#endif
    }

    //try to initiate WifiScanner object in Java
    private bool GetWifiManager()
    {
        //AndroidJavaClass scannerClass = new AndroidJavaClass("com.example.wifiscanner");
        scannerObject = new AndroidJavaObject("com.example.wifiscanner.WifiScanner", unityActivity);

        if(scannerObject != null)
        {
            bool sucess = scannerObject.Call<bool>("isManagerSetUpSuccess");
            if (sucess)
            {
                //toastMaker.CallToast("yaay");
                if (faketoast != null)
                    faketoast.text = "yaay";
            }
            else
            {
                //toastMaker.CallToast("fuck");
                if (faketoast != null)
                    faketoast.text = "fuck";

            }

            return sucess;
        }
        else
        {
            if (faketoast != null)
                faketoast.text = "feck";
            //toastMaker.CallToast("feck");
        }

        return false;
    }

    //try to scan for wifi networks
    private bool ScanForNetworks()
    {
        if (scannerObject.Call<bool>("isListContentChanged"))
        {
            return true;
        }

        return scannerObject.Call<bool>("scanWifi");        
    }

    private void FetchNetworkList()
    {
        if(success_test == false || !scannerObject.Call<bool>("isListContentChanged")){return;}

        AndroidJavaObject javaListOfNetworks = scannerObject.Call<AndroidJavaObject>("getListOfNetworks");
        if(javaListOfNetworks == null)
        {
            text.text = "list ain't workin :(";
            return;
        }

        text.text = "";

        int count = javaListOfNetworks.Call<int>("size");
        listOfNetworks = new List<WifiScanResults>();
        for(int i = 0; i < count; i++)
        {
            string[] result = javaListOfNetworks.Call<string>("get", i).Split();
            if(result.Length == 4)
            {
                listOfNetworks.Add(new WifiScanResults(result[0], result[1], result[2], result[3]));
                //text.text = text.text + listOfNetworks[listOfNetworks.Count - 1].SSID + " " + listOfNetworks[listOfNetworks.Count - 1].GetWifiDistance() + " \n";
            }
        }
        pc.FillListOfNetworks(listOfNetworks);
        text.text = text.text + listOfNetworks[1].BSSID + " " + listOfNetworks[1].signalLevel + " " + listOfNetworks[1].signalFrequency + " \n";
        text.text = text.text + listOfNetworks[1].BSSID + " " + listOfNetworks[1].GetWifiDistance();
        object[] boolParams = new object[1];
        boolParams[0] = false;
        scannerObject.Call("setListContentChanged", boolParams);
    }

    public void TurnOnTimer(bool state)
    {
        timerOn = state;
        scanTimer = timeBetweenScans;
    }

    public void Update()
    {
        if (wifiManagerActive == false)
        {
            wifiManagerActive = GetWifiManager();
            return;
        }

        if (scanTimer > 0)
        {
            if (timerOn)
                scanTimer -= Time.deltaTime;
            if (scanTimer < 10f && !scannerObject.Call<bool>("isListContentChanged"))
                text.text = "Scanning";
        }
        else
        {
            success_test = ScanForNetworks();
            if (faketoast != null)
                faketoast.text = "" + success_test;
            scanTimer = timeBetweenScans;
        }

        FetchNetworkList();
        timer.text = "" + scanTimer;
    }

}
