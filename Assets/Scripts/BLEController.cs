using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BLEController : MonoBehaviour
{
    private List<string> fingerAngles = Enumerable.Repeat("No angle", 5).ToList();
    private List<string> mpuValues = Enumerable.Repeat("No value", 6).ToList();

    private const string bleFrameworkPathName = "com.carolinapistillo.ble_plugin.BLEPlugin";
    public const float UPDATE_RATE = 0.01f; // Seconds

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
            //Debug.Log("string: " + message);
            string[] values = message.Split(' ');
            if(values.Length != 8)
                return;
            foreach (string value in values) {
                int parsedValue;
                if(!int.TryParse(value, out parsedValue)) {
                    print("Parsing error catched: " + value);
                    return;
                }
            }

            fingerAngles[0] = values[0];
            fingerAngles[1] = values[1];
            fingerAngles[2] = values[2];
            fingerAngles[3] = values[3];
            fingerAngles[4] = values[4];

            mpuValues[0] = values[5];
            mpuValues[1] = values[6];
            mpuValues[2] = values[7];
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

    public int getFingerAngle(int index) {
        return int.Parse(fingerAngles[index]);
    }

    public int getMpuValue(int index) {
        return int.Parse(mpuValues[index]);
    }

    public bool anglesNotAvailable() {
        foreach (string angle in fingerAngles) {
            if (angle == "No angle") {
                return true;
            }
        }
        return false;
    }

    public bool mpuValuesNotAvailable() {
        foreach (string mpuValue in mpuValues) {
            if (mpuValue == "Novalue") {
                return true;
            }
        }
        return false;
    }
}
