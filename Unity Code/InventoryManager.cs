using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public PlayerController player;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;
    public TMP_Text carrotText;
    public TMP_Text anuletText;
    public TMP_Text stabilizerText;
    public TMP_Text drinkText;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        UpdateInventory();
    }

    private void OnEnable()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        if (player.playerItems.carrot >0)
            btn1.interactable = true;
        else
            btn1.interactable = false;

        if (player.playerItems.amuletCoin > 0)
            btn2.interactable = true;
        else
            btn2.interactable = false;

        if (player.playerItems.matterStabilizer > 0)
            btn3.interactable = true;
        else
            btn3.interactable = false;

        if (player.playerItems.energyDrinks > 0)
            btn4.interactable = true;
        else
            btn4.interactable = false;

        carrotText.text = ""+ player.playerItems.carrot;
        anuletText.text = "" + player.playerItems.amuletCoin;
        stabilizerText.text = "" + player.playerItems.matterStabilizer;
        drinkText.text = "" + player.playerItems.energyDrinks;
    }

    public void UseItem(int item)
    {
        switch (item)
        {
            case 0:
                player.battleInformation.stopRotationChange = true;
                player.playerItems.carrot--;
                break;
            case 1:
                player.battleInformation.SetRewardIncreaseAmount(100);
                player.playerItems.amuletCoin--;
                break;
            case 2:
                player.battleInformation.stopPhasingOut = true;
                player.playerItems.matterStabilizer--;
                break;
            default:
                break;
        }
        UpdateInventory();
    }
}
