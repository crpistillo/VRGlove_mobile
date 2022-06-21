using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLEController : MonoBehaviour
{
    private string angle = "No angle";
    private const string bleFrameworkPathName = "com.carolinapistillo.ble_plugin.BLEPlugin";
    public const float UPDATE_RATE = 0.25f; // Seconds

    class UnityCallback : AndroidJavaProxy
    {
        private System.Action<string> initializeHandler;
        public UnityCallback(System.Action<string> initializeHandlerIn) : base(bleFrameworkPathName + "$UnityCallback")
        {
            initializeHandler = initializeHandlerIn;
        }
        public void sendMessage(string message)
        {
            if (initializeHandler != null)
            {
                initializeHandler(message);
            }
        }
    }

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;

    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass(bleFrameworkPathName);
            }
            return _pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (_pluginInstance == null)
            {
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance", activity);
            }
            return _pluginInstance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        System.Action<string> onReadCharacteristic = ((string message) =>
        {
            angle = message;
            //Debug.Log("UNITY, Angle: " + message);
        });
        PluginInstance.Call("connectUnityCallbacks", new object[] { new UnityCallback(onReadCharacteristic)});
        PluginInstance.Call("prepareAndStartBleScan");
    }

    // Update is called once per frame
    void Update()
    {
        if(!PluginInstance.Call<bool>("setUpFinished")) {
            return;
        }
        PluginInstance.Call("readCharacteristic");
    }

    public float getAngle() {
        return float.Parse(angle);
    }

     public string getStringAngle() {
        return angle;
    }
}
