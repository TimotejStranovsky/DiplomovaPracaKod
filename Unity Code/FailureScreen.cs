using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailureScreen : MonoBehaviour
{
    [SerializeField] TMP_Text failureText;
    [SerializeField] Button retry;
    [SerializeField] Image energyDrink;
    PlayerController player;
    Enemy ghost;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ghost = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        failureText.text = ghost.enemyName+" has escaped!";
        if(player.playerItems.energyDrinks <= 0)
        {
            Debug.Log("Bloop");
            retry.interactable = false;
            energyDrink.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            Debug.Log("Bleep");
            retry.interactable = true;
            energyDrink.color = Color.white;
        }
    }

    public void ChangeScene()
    {
        var wifiScanner = player.GetComponent<WifiScanner>();
        wifiScanner.TurnOnTimer(true);
        player.transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadScene("LevelGenerationTest");
    }

    public void Retry()
    {
        if (player.playerItems.energyDrinks > 0)
        {
            player.playerItems.energyDrinks--;
            SceneManager.LoadScene("BlankAR");
        }
    }
}
