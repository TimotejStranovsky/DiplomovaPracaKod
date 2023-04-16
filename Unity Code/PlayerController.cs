using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public string RSSI1;
    public string RSSI2;
    public string RSSI3;
    public string enemyName;
    public BattleInformation battleInformation;
    public int money;
    public PlayerItems playerItems;
    public int CapturedMonsters
    {
        set
        {
            capturedMonsters = value;
        }
    }
    public DatabaseManager database;

    List<WifiScanResults> listOfNetworks;
    int capturedMonsters = 0;

    //plz delet
    public void SwitchScene(string enemy)
    {
        enemyName = enemy;
        SceneManager.LoadScene("BlankAR");
    }

    public void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    public void FillListOfNetworks(List<WifiScanResults> scannedNetworks)
    {
        listOfNetworks = new List<WifiScanResults>(scannedNetworks);
        string conditions = "";
        foreach(var network in listOfNetworks)
        {
            conditions += network.BSSID + ", ";
        }
        conditions = conditions.Substring(0, conditions.Length - 2);
        database.CallGetTestPoints(this, conditions);
    }

    public void CalculatePosition(List<TestingPoint> testingPoints)
    {
        //FillListOfNetworks();
        foreach(var testingPoint in testingPoints)
        {
            testingPoint.CalculateDistance(listOfNetworks);
        }
        testingPoints.Sort(SortPointsByDistance);
        float x = (testingPoints[0].x + testingPoints[1].x + testingPoints[2].x) / 3f;
        float y = (testingPoints[0].y + testingPoints[1].y + testingPoints[2].y) / 3f;
        Debug.Log("-------------------------");
        Debug.Log(testingPoints[0].id + " " + testingPoints[0].x + " " + testingPoints[0].y + " " + testingPoints[0].distance);
        Debug.Log(testingPoints[1].id + " " + testingPoints[1].x + " " + testingPoints[1].y + " " + testingPoints[1].distance);
        Debug.Log(testingPoints[2].id + " " + testingPoints[2].x + " " + testingPoints[2].y + " " + testingPoints[2].distance);
        Debug.Log(x + " " + y);

        transform.position = new Vector3(x, y, transform.position.z);
    }

    private static int SortPointsByDistance(TestingPoint x, TestingPoint y)
    {
        if (x == null)
        {
            if (y == null)
            {
                // If x is null and y is null, they're
                // equal.
                return 0;
            }
            else
            {
                // If x is null and y is not null, y
                // is greater.
                return -1;
            }
        }
        else
        {
            // If x is not null...
            //
            if (y == null)
            // ...and y is null, x is greater.
            {
                return 1;
            }
            else
            {
                if (x.distance > y.distance)
                {
                    // If the strings are not of equal length,
                    // the longer string is greater.
                    //
                    return 1;
                }
                else
                {
                    if (x.distance == y.distance)
                    {
                        return 0;
                    }
                    else 
                    { 
                        return -1; 
                    }
                }
            }
        }
    }
}
