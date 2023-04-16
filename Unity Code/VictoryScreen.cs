using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] TMP_Text ghostName;
    [SerializeField] TMP_Text money;
    [SerializeField] Image ghostImage;
    PlayerController player;
    Enemy ghost;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ghost = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        ghostName.text = ghost.enemyName;
        money.text = ""+ghost.reward;
        ghostImage.sprite = ghost.ghostImg;
        player.money += ghost.reward;
    }

    public void ChangeScene()
    {
        var wifiScanner = player.GetComponent<WifiScanner>();
        wifiScanner.TurnOnTimer(true);
        player.transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadScene("LevelGenerationTest");
    }
}
