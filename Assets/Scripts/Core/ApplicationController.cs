using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    /// <summary>
    /// Quality suffix of assets to load.
    /// </summary>
    public string Quality = "hd";

    /// <summary>
    /// The target platform this application is running on.
    /// </summary>
    public string Platform = "";

    /// <summary>
    /// If true, remote assets will be loaded from the static content cdn.
    /// </summary>
    public bool Remote = false;

    /// <summary>
    /// Unique resource identifier where remote static content resides.
    /// </summary>
    public string RemoteUri = "https://neatocdn.blob.core.windows.net/static/";


    // COMPONENT
    // ========================================================================

    /// <summary>
    /// Component start routine.
    /// </summary>
	void Start ()
	{
        // Keep the application controller persistant throughout the entire 
        // application lifecycle.
        DontDestroyOnLoad(this);

        // Detect platform
        DetectPlatform();
	}


    // PRIVATE
    // ========================================================================

    /// <summary>
    /// Detects the running platform. Derived string is used as a prefix for
    /// AssetBundles that are later loaded.
    /// </summary>
    private void DetectPlatform()
    {
        switch(Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                Platform = "windows";
                break;

            case RuntimePlatform.Android:
                Platform = "android";
                break;

            case RuntimePlatform.IPhonePlayer:
                Platform = "ios";
                break;

            default:
                Platform = "unknown";
                break;
        }

        Debug.Log("Detected platform: " + Platform);
    }
}
