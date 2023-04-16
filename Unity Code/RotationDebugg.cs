using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotationDebugg : MonoBehaviour
{
    [SerializeField] RotateAroundTarget rotateAroundTarget;
    [SerializeField] GameObject speedText;
    [SerializeField] GameObject speedVerticalText;
    [SerializeField] GameObject heightText;
    [SerializeField] GameObject switchText;
    [SerializeField] GameObject phaseText;
    [SerializeField] Toggle switchToggle;
    [SerializeField] Toggle phaseToggle;

    public void OnEnable()
    {
        Time.timeScale = 0;


        speedText.GetComponent<TMP_InputField>().text = ""+rotateAroundTarget.speed;
        speedVerticalText.GetComponent<TMP_InputField>().text = "" + rotateAroundTarget.vSpeed;
        heightText.GetComponent<TMP_InputField>().text = "" + rotateAroundTarget.maxHeight;
        switchText.GetComponent<TMP_InputField>().text = "" + rotateAroundTarget.switchTimerMax;
        phaseText.GetComponent<TMP_InputField>().text = "" + rotateAroundTarget.phaseTimerMax;
        switchToggle.isOn = rotateAroundTarget.switchesRotation;
        phaseToggle.isOn = rotateAroundTarget.phasesOut;
    }

    public void Submit()
    {
        rotateAroundTarget.speed = int.Parse(speedText.GetComponent<TMP_InputField>().text);
        rotateAroundTarget.vSpeed = float.Parse(speedVerticalText.GetComponent<TMP_InputField>().text);
        rotateAroundTarget.maxHeight = float.Parse(heightText.GetComponent<TMP_InputField>().text);

        if (switchToggle.isOn)
        {
            rotateAroundTarget.switchesRotation = true;
            rotateAroundTarget.switchTimerMax = float.Parse(switchText.GetComponent<TMP_InputField>().text);
        }

        if (phaseToggle.isOn)
        {
            rotateAroundTarget.phasesOut = true;
            rotateAroundTarget.phaseTimerMax = float.Parse(phaseText.GetComponent<TMP_InputField>().text);
        }

        Time.timeScale = 1;
        rotateAroundTarget.ResetTarget();
        gameObject.SetActive(false);
    }
}
