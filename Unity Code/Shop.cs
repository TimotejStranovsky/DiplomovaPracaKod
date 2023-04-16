using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public PlayerController player;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        UpdateListing();
    }

    private void UpdateListing()
    {
        if (player.money < 300)
            btn4.interactable = false;
        if (player.money < 200)
            btn3.interactable = false;
        if (player.money < 100)
            btn2.interactable = false;
        if (player.money < 50)
            btn1.interactable = false;
    }

    public void BuyItem(int itemNum)
    {
        switch (itemNum)
        {
            case 0:
                if(player.money >= 50)
                {
                    player.money -= 50;
                    player.playerItems.carrot++;
                }
                break;
            case 1:
                if (player.money >= 100)
                {
                    player.money -= 100;
                    player.playerItems.amuletCoin++;
                }
                break;
            case 2:
                if (player.money >= 200)
                {
                    player.money -= 200;
                    player.playerItems.matterStabilizer++;
                }
                break;
            default:
                if (player.money >= 300)
                {
                    player.money -= 300;
                    player.playerItems.energyDrinks++;
                }
                break;
        }
        UpdateListing();
    }

    public void ExitShop()
    {
        SceneManager.UnloadSceneAsync("Shop");
    }
}
