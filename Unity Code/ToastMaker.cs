using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastMaker : MonoBehaviour
{
    private const string ToastShort = "LENGTH_SHORT";
    private const string ToastLong = "LENGTH_LONG";
    AndroidJavaObject unityActivity;
    // Start is called before the first frame update
    void Start()
    {
        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
    }

    public void CallToast(string toastText, bool isLong = true)
    {
#if UNITY_ANDROID
        if (isLong)
        {
            ShowAndroidToast(toastText, ToastLong);
        }
        else
        {
            ShowAndroidToast(toastText, ToastShort);
        }
#else
		Debug.Log("No Toast setup for this platform.");
#endif
    }
    private void ShowAndroidToast(string toastText, string toastLength = ToastLong)
    {
        //create a Toast class object
        AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");

        //create an array and add params to be passed
        object[] toastParams = new object[3];
        toastParams[0] = unityActivity;
        toastParams[1] = toastText;
        toastParams[2] = toastClass.GetStatic<int>(toastLength);

        //call static function of Toast class, makeText
        AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", toastParams);

        //show toast
        toastObject.Call("show");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
