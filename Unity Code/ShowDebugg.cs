using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDebugg : MonoBehaviour
{
    public GameObject debuggUI;

    public void TurnOnDebugg()
    {
        debuggUI.SetActive(true);
    }
}
