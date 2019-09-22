using UnityEngine;

public class AdsHelper
{
    public static string getDeviceAdMobID()
    {
        if (UnityEngine.Debug.isDebugBuild)
        {
#if UNITY_ANDROID
            return GetAndroidAdMobID();
#elif UNITY_IPHONE
            return GetIOSAdMobID();
#else
            return null;
#endif
        }
        else
            return null;
    }

    public static string GetAndroidAdMobID()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityEngine.AndroidJavaClass up = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
            UnityEngine.AndroidJavaObject currentActivity = up.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
            UnityEngine.AndroidJavaObject contentResolver = currentActivity.Call<UnityEngine.AndroidJavaObject>("getContentResolver");
            UnityEngine.AndroidJavaObject secure = new UnityEngine.AndroidJavaObject("android.provider.Settings$Secure");
            string deviceID = secure.CallStatic<string>("getString", contentResolver, "android_id");
            return Md5Sum(deviceID).ToUpper();
        }
        else return null;
    }

    public static string GetIOSAdMobID()
    {
#if UNITY_IPHONE
        return Md5Sum(UnityEngine.iOS.Device.advertisingIdentifier);
#else
        return null;
#endif
    }

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}