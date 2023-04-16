using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostBtn : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] string ghostName;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void GenerateGhost(int minRoll, int maxRoll)
    {
        var result = Random.Range(minRoll, maxRoll);
        switch(result)
        {
            case 0:
                ghostName = "Spookz";
                break;
            case 1:
                ghostName = "Ooky Spooky";
                break;

        }
    }

    public void EncounterGhost()
    {
        var wifiScanner = player.GetComponent<WifiScanner>();
        wifiScanner.TurnOnTimer(false);
        player.enemyName = ghostName;
        player.transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("BlankAR");
    }
}
