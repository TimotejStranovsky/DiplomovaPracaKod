using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenShop : MonoBehaviour
{
    public GameObject inventory;

    public void OpensShop()
    {
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
        OpenInventory();
    }

    public void OpenEnemyIndex()
    {
        SceneManager.LoadScene("Index", LoadSceneMode.Additive);
    }

    public void OpenInventory()
    {
        if (inventory.gameObject.activeSelf)
            inventory.gameObject.SetActive(false);
        else
            inventory.gameObject.SetActive(true);
    }
}
