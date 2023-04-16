using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPoz : MonoBehaviour
{
    public TMP_Text textX;
    public TMP_Text textY;
    public Transform playerPoz;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textX.text = "" + playerPoz.position.x;
        textY.text = "" + playerPoz.position.y;
    }
}
