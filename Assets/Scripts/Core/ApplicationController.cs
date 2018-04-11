using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// Next scene to be loaded.
    /// </summary>
    public string NextScene = "Zombieland";


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

        // Load next scene
        LoadNextScene();
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
                Platform = "StandaloneWindows";
                break;

            case RuntimePlatform.Android:
                Platform = "Android";
                break;

            case RuntimePlatform.IPhonePlayer:
                Platform = "iOS";
                break;

            case RuntimePlatform.WebGLPlayer:
                Platform = "WebGL";
                break;

            default:
                Platform = "unknown";
                break;
        }

        Debug.Log("Detected platform: " + Platform);
    }

    /// <summary>
    /// Loads the next scene.
    /// </summary>
    private void LoadNextScene()
    {
        if(Remote)
        {
            StartCoroutine(LoadRemoteAssetBundle());
        }
        else
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Path.Combine(@"AssetBundles\"+Platform, NextScene.ToLower() + "." + Quality));
            request.completed += OnAssetBundleLoadComplete;
        }
    }

    /// <summary>
    /// Loads a remote asset bundle.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadRemoteAssetBundle()
    {
        UnityWebRequest www = UnityWebRequest.GetAssetBundle(RemoteUri+Platform+"/"+NextScene.ToLower() +"."+ Quality);

        yield return www.SendWebRequest();

        Debug.Log("Progress: " + www.downloadProgress);

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            SceneManager.LoadScene(NextScene);
        }
    }


    // EVENTS
    // ========================================================================

    private void OnAssetBundleLoadComplete(AsyncOperation obj)
    {
        SceneManager.LoadScene(NextScene);
    }
}
