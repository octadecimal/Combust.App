using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

class CombustCustomBuildProcessor : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        // TODO: Clear all sprite renderers in all scenes

        //Debug.Log("CombustCustomBuildProcessor.OnPreprocessBuild for target " + target + " at path " + path);
    }
}