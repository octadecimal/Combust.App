using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class CombustSpriteTools : Editor
{
    /// <summary>
    /// Clears all sprite renderers.
    /// </summary>
    [MenuItem("Combust/Sprites/Clear")]
    public static void Clear()
    {
        // Unload all asset bundles
        AssetBundle.UnloadAllAssetBundles(true);

        // Get all sprite renderers
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();

        Debug.Log("Clearing sprite renderers: " + renderers.Length);

        // Clear each sprite renderer
        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.sprite = null;
        }
    }

    /// <summary>
    /// Shows HD sprite assets.
    /// </summary>
    [MenuItem("Combust/Sprites/HD")]
    public static void ShowHD()
    {
        ShowQuality("hd");
    }

    /// <summary>
    /// Shows SD sprite assets.
    /// </summary>
    [MenuItem("Combust/Sprites/SD")]
    public static void ShowSD()
    {
        ShowQuality("sd");
    }

    /// <summary>
    /// Shows sprite assets by quality suffix.
    /// </summary>
    /// <param name="quality"></param>
    private static void ShowQuality(string quality)
    {
        // Clear
        Clear();

        // Get current game name
        string gameName = FindObjectOfType<SlotGameController>().GameName;

        // Create request
        var request = AssetBundle.LoadFromFileAsync(Path.Combine("AssetBundles/StandaloneWindows", "zombieland."+quality));
        request.completed += OnAssetBundleLoadComplete;
    }

    /// <summary>
    /// Asset bundle load complete.
    /// </summary>
    /// <param name="obj"></param>
    private static void OnAssetBundleLoadComplete(AsyncOperation obj)
    {
        // Clean
        obj.completed -= OnAssetBundleLoadComplete;

        // Get all sprite renderers
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();

        // Handle each sprite renderer
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.sprite = GetSpriteFromAssetBundle(renderer.gameObject.name);
        }
    }

    /// <summary>
    /// Gets a sprite, by name, from any loaded asset bundles.
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    private static Sprite GetSpriteFromAssetBundle(string assetName)
    {
        // Get all loaded asset bundles
        IEnumerable<AssetBundle> bundles = AssetBundle.GetAllLoadedAssetBundles();

        // Search for sprite in bundles
        foreach (var bundle in bundles)
        {
            Sprite sprite = bundle.LoadAsset<Sprite>(assetName);

            if(sprite)
            { 
                return sprite;
            }
        }

        // Sprite not found
        Debug.LogError("Sprite not found: " + assetName);
        return null;
    }
}
