using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ensure that all Scenes are added to the Build Settings Window
/// Ensure that Multi Screen Setup scene will be the first loaded scene in the list, we ignore Split Screen Setup scene
/// </summary>
public class BuildSingleRuntimeWithOneScreenPerScreen : MonoBehaviour
{
    /// <summary>
    /// Sets a menu option under Build. 
    /// </summary>
    //[MenuItem("Build/Build Windows - Single Runtime With Multi Displays")]
    public static void BuildMultiScreen(BuildTarget PlatformBuildTarget)
    {
        // Creates a new BuildPlayerOptions
        var buildPlayerOptions = new BuildPlayerOptions
        {
            // Sets the scenes to be built in this build.
            // In this instance we are loading all scenes that are in the build settings windows. 
            // Alternate way to do this is creating an array with specific scenes you would want to load, 
            // Exmaple: buildPlayerOptions.scenes = new[] { "Assets/Scenes/SplitScreen/SplitScreenScene_Setup.unity" };
            scenes = ReturnAllScenePathsInEditorBuildSettings(),

            //set the path and get the extension for the target platform
            locationPathName = BuildLocationPath() + BuildWindow_CustomEditor.GetExtensionForBuildTarget(PlatformBuildTarget),
            target = PlatformBuildTarget,
            options = BuildOptions.None
        };

        // Builds here, then creates a build report. 
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    /// <summary>
    /// Returns an array of all scene paths that is in the EditorBuildSettings
    /// Ignores the scene name that is "SplitScreenSetup"
    /// </summary>
    private static string[] ReturnAllScenePathsInEditorBuildSettings()
    {
        var Paths = new List<string>();
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {
                if (NameFromIndex(i) != "SplitScreenSetup")
                {
                    Paths.Add(EditorBuildSettings.scenes[i].path);
                }
            }
        }

        return Paths.ToArray();
    }

    /// <summary>
    /// Returns the name of a scene without the path
    /// </summary>
    private static string NameFromIndex(int BuildIndex)
    {
        var path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        var slash = path.LastIndexOf('/');
        var name = path.Substring(slash + 1);
        var dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    /// <summary>
    /// Location of build 
    /// </summary>
    public static string BuildLocationPath()
    {
        return "Builds/SingleRuntimeWithOneScreenPerScreen/MultiScreenBuild";
    }

}
