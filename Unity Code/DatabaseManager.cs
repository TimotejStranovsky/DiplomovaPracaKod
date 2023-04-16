using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public enum Status
{
	haventRun,
	isRunning,
	finished
}

public class DatabaseManager : MonoBehaviour
{
	public string localhost;
	public string getTestingPointsURL;
	public string getZoneURL;
	public string getConnectedZonesURL;
	public TMP_Text text;
	public TMP_Text text2;
	public PlayerController player;
	public WorldBuilder builder;
	//public ToastMaker toast;

	[SerializeField] JSONReader JSONDatabase;

	List<TestingPoint> testingPoints;
	List<ConnectedZone> connectedZones;
	Zone foundZone;
	Status isTestPointsRunning;
	Status isZoneRunning;
	Status isConnectedZonesRunning;

	private void Awake()
	{
		getTestingPointsURL = localhost + "/UnityDatabaseTest/getTestingPoints.php?condition=";
		getZoneURL = localhost + "/UnityDatabaseTest/getZone.php?condition=";
		getConnectedZonesURL = localhost + "/UnityDatabaseTest/getConnectedZones.php?condition=";
		//toast.CallToast("boop", false);
	}
	public void CallTest()
    {
		StartCoroutine(TestConnection());
    }
	IEnumerator TestConnection()
	{
		string url = "https://www.google.com/";
		UnityWebRequest hs_get = UnityWebRequest.Get(url);
		yield return hs_get.SendWebRequest();
		text.text = "Complete";
		if (hs_get == null)
		{
			text.text = "hs_get didn't run";
		}
		if (hs_get.error != null)
		{
			text.text = "There was an error getting the high score: " + hs_get.error;
			//toast.CallToast(hs_get.error);
		}
	}

		public void TestToast()
    {
		//toast.CallToast("boop", true);
	}

	public void GetTestPointsBtn()
	{
		CallGetTestPoints(player, "'1','2','3'");
	}
	public void CallGetTestPoints(PlayerController playerController, string conditions)
    {
		if (testingPoints == null)
		{
			testingPoints = new List<TestingPoint>();
		}
		player = playerController;
		//StartCoroutine(GetTestPoints(conditions));
		JSONGetTestPoints(conditions);
	}
	public void CallGetZone(string zoneId, WorldBuilder worldBuilder)
	{
		builder = worldBuilder;
		//StartCoroutine(GetZone(zoneId));
		JSONGetZone(int.Parse(zoneId));
	}
	public void CallGetConnectedZones(string zoneId, WorldBuilder worldBuilder)
	{
		if (connectedZones == null)
		{
			connectedZones = new List<ConnectedZone>();
		}
		builder = worldBuilder;
		//StartCoroutine(GetConnectedZones(zoneId));
		JSONGetConnectedZones(int.Parse(zoneId));
	}

	IEnumerator GetTestPoints(string BSSIDs)
	{
		string url = getTestingPointsURL + UnityWebRequest.EscapeURL(BSSIDs);
		UnityWebRequest hs_get = UnityWebRequest.Get(url);
		isTestPointsRunning = Status.isRunning;
		yield return hs_get.SendWebRequest();
		if (hs_get == null)
		{
			//text.text = "hs_get didn't run";
		}
		if (hs_get.error != null)
		{
			//text.text = "There was an error getting the high score: " + hs_get.error;
			//toast.CallToast(hs_get.error);
		}
		else
		{
			//text.text = "";
			testingPoints.Clear();
			string dataText = hs_get.downloadHandler.text;
			MatchCollection mc = Regex.Matches(dataText, @"~");
			if (mc.Count > 0)
			{
				//Debug.Log(mc.Count);
				string[] siteData = Regex.Split(dataText, @"~");
				for (int i = 0; i < mc.Count; i++)
				{
					string[] splitData = Regex.Split(siteData[i], @"_");
					if (splitData.Length == 5)
					{
						//if(testingPoints == null)
						//                  {
						//	testingPoints = new List<TestingPoint>();
						//	testingPoints.Add(new TestingPoint(splitData[0], splitData[1], splitData[2]));
						//                  }

						if (testingPoints.Count < 1 || testingPoints[testingPoints.Count - 1].id != splitData[0])
						{
							testingPoints.Add(new TestingPoint(splitData[0], splitData[1], splitData[2]));
						}

						testingPoints[testingPoints.Count - 1].accessPoints.Add(new AccessPoint(splitData[3], splitData[4]));
						//Debug.Log(testingPoints[testingPoints.Count - 1].id + " " + testingPoints[testingPoints.Count - 1].x + " " + testingPoints[testingPoints.Count - 1].y + " " + testingPoints[testingPoints.Count - 1].accessPoints[testingPoints[testingPoints.Count - 1].accessPoints.Count - 1].BSSID + " " + testingPoints[testingPoints.Count - 1].accessPoints[testingPoints[testingPoints.Count - 1].accessPoints.Count - 1].RSSI);

					}
					//for(int j =0; j< splitData.Length; j++)
					//{
					//	text.text += splitData[j] + " ";//splitData
					//}
					//text.text += "\n";
				}
			}
			player.CalculatePosition(testingPoints);
			isTestPointsRunning = Status.finished;
		}
	}

	IEnumerator GetZone(string zoneID)
	{
		string url = "http://" + getZoneURL + UnityWebRequest.EscapeURL(zoneID);// localhost + "/UnityDatabaseTest/getZone.php?condition="+zoneID
		text.text = url;
		UnityWebRequest hs_get = UnityWebRequest.Get(url);
		isZoneRunning = Status.isRunning;
		yield return hs_get.SendWebRequest();
		if (hs_get == null)
		{
			text.text = "hs_get didn't run";
		}
		if (hs_get.error != null)
		{
			//text.text = "There was an error getting the high score: " + hs_get.error;
			//toast.CallToast(hs_get.error);
		}
		else
		{
			string dataText = hs_get.downloadHandler.text;
			Debug.Log(dataText);
			//Debug.Break();
			MatchCollection mc = Regex.Matches(dataText, @"_");
			if (mc.Count > 0)
			{
				//Debug.Log(mc.Count);
				//mc.ToString();
				string[] siteData = Regex.Split(dataText, @"_");
				for (int i = 0; i < mc.Count - 1; i++)
				{
					//Debug.Log(siteData[0] + " " + siteData[1] + " " + siteData[2]);
					foundZone = new Zone(siteData[0], siteData[1], siteData[2], siteData[3], siteData[4], siteData[5]);
					text.text = foundZone.ToString();
				}
			}
			//do something with world builder
			builder.BeginGenerateMap(foundZone);
			isZoneRunning = Status.finished;
		}
	}

	IEnumerator GetConnectedZones(string zoneID)
	{
		string url = getConnectedZonesURL + UnityWebRequest.EscapeURL(zoneID);
		UnityWebRequest hs_get = UnityWebRequest.Get(url);
		isTestPointsRunning = Status.isRunning;
		yield return hs_get.SendWebRequest();
		if (hs_get == null)
		{
			text.text = "hs_get didn't run";
		}
		if (hs_get.error != null)
		{
			text.text = "There was an error getting the high score: " + hs_get.error;
			//toast.CallToast(hs_get.error);
		}
		else
		{
			//text.text = "";
			connectedZones.Clear();
			string dataText = hs_get.downloadHandler.text;
			MatchCollection mc = Regex.Matches(dataText, @"~");
			if (mc.Count > 0)
			{
				//Debug.Log(mc.Count);
				string[] siteData = Regex.Split(dataText, @"~");
				for (int i = 0; i < mc.Count; i++)
				{
					string[] splitData = Regex.Split(siteData[i], @"_");
					if (splitData.Length == 5)
					{
						connectedZones.Add(new ConnectedZone(new Zone(splitData[0], splitData[1], splitData[2], splitData[3], splitData[4], siteData[5]), siteData[6], siteData[7]));
					}
				}
			}
			//player.CalculatePosition(testingPoints);
			builder.BeginGeneratingConnected(int.Parse(zoneID),connectedZones);
			isTestPointsRunning = Status.finished;
		}
	}

	void JSONGetTestPoints(string BSSIDs)
	{
		//text.text = "";
		testingPoints.Clear();

		string[] filter = Regex.Split(BSSIDs, @",");
		string dataText = JSONDatabase.GetTP(filter);
		MatchCollection mc = Regex.Matches(dataText, @"~");
		if (mc.Count > 0)
		{
			//Debug.Log(mc.Count);
			string[] siteData = Regex.Split(dataText, @"~");
			for (int i = 0; i < mc.Count; i++)
			{
				string[] splitData = Regex.Split(siteData[i], @"_");
				if (splitData.Length == 5)
				{
					//if(testingPoints == null)
					//                  {
					//	testingPoints = new List<TestingPoint>();
					//	testingPoints.Add(new TestingPoint(splitData[0], splitData[1], splitData[2]));
					//                  }

					if (testingPoints.Count < 1 || testingPoints[testingPoints.Count - 1].id != splitData[0])
					{
						testingPoints.Add(new TestingPoint(splitData[0], splitData[1], splitData[2]));
					}

					testingPoints[testingPoints.Count - 1].accessPoints.Add(new AccessPoint(splitData[3], splitData[4]));
					//Debug.Log(testingPoints[testingPoints.Count - 1].id + " " + testingPoints[testingPoints.Count - 1].x + " " + testingPoints[testingPoints.Count - 1].y + " " + testingPoints[testingPoints.Count - 1].accessPoints[testingPoints[testingPoints.Count - 1].accessPoints.Count - 1].BSSID + " " + testingPoints[testingPoints.Count - 1].accessPoints[testingPoints[testingPoints.Count - 1].accessPoints.Count - 1].RSSI);

				}
				//for(int j =0; j< splitData.Length; j++)
				//{
				//	text.text += splitData[j] + " ";//splitData
				//}
				//text.text += "\n";
			}
		}
		player.CalculatePosition(testingPoints);
		isTestPointsRunning = Status.finished;
	}

	void JSONGetZone(int zoneID)
	{
		string dataText = JSONDatabase.GetZones(zoneID);
		//Debug.Log(dataText);
		//Debug.Break();
		MatchCollection mc = Regex.Matches(dataText, @"_");
		if (mc.Count > 0)
		{
			//Debug.Log(mc.Count);
			//mc.ToString();
			string[] siteData = Regex.Split(dataText, @"_");
			for (int i = 0; i < mc.Count - 1; i++)
			{
				//Debug.Log(siteData[0] + " " + siteData[1] + " " + siteData[2]);
				foundZone = new Zone(siteData[0], siteData[1], siteData[2], siteData[3], siteData[4], siteData[5]);
				text.text = foundZone.ToString();
			}
			//do something with world builder
			builder.BeginGenerateMap(foundZone);
			isZoneRunning = Status.finished;
		}
	}

	void JSONGetConnectedZones(int zoneID)
	{
		//text.text = "";
		connectedZones.Clear();
		string dataText = JSONDatabase.GetConnectedZones(zoneID);
		MatchCollection mc = Regex.Matches(dataText, @"~");
		if (mc.Count > 0)
		{
			//Debug.Log(mc.Count);
			string[] siteData = Regex.Split(dataText, @"~");
			for (int i = 0; i < mc.Count; i++)
			{
				string[] splitData = Regex.Split(siteData[i], @"_");
                if (splitData.Length == 8)
				{
					connectedZones.Add(new ConnectedZone(new Zone(splitData[0], splitData[1], splitData[2], splitData[3], splitData[4], splitData[5]), splitData[6], splitData[7]));
				}
			}
		}
		//player.CalculatePosition(testingPoints);
		builder.BeginGeneratingConnected(zoneID, connectedZones);
		isTestPointsRunning = Status.finished;
	}

	//  public void Update()
	//  {
	//text2.text = "isRunning " + isRunning;
	//  }
}
