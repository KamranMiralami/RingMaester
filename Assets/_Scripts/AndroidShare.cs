using UnityEngine;

public class AndroidShare
{
    public static void ShareText(string message)
    {
        // Only works on Android
        if (Application.platform != RuntimePlatform.Android)
            return;
        try
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share via");
            currentActivity.Call("startActivity", chooser);
        }
        catch (System.Exception e)
        {
            Debug.Log("Error sharing text: " + e.Message);
        }
    }
}
