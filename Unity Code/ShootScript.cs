using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class ShootScript : MonoBehaviour
{
    //[SerializeField] Button fireButton;
    [SerializeField] Image crosshairs;
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] Camera arCamera;
    int score;
    private ARRaycastManager aRRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool isFireHeld;
    // Start is called before the first frame update
    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        //fireButton.onClick.AddListener(Fire);
        score = 0;
        scoreUI.text = "" + score;
        isFireHeld = false;
    }

    // Update is called once per frame
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("FIRE START");
            isFireHeld = true;
        }

        if (context.canceled)
        {
            Debug.Log("FIRE END");
            isFireHeld = false;
        }
    }

    public void FixedUpdate()
    {
        if (isFireHeld)
        {
            Ray ray = arCamera.ScreenPointToRay(crosshairs.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,6))
            {
                Enemy hitObject = hit.transform.gameObject.GetComponentInParent<Enemy>();
                if (hitObject != null)
                {
                    //score++;
                    //scoreUI.text = "" + score;
                    hitObject.GetHit(0.1f);
                }
            }
        }
    }
}
