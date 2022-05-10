using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Ensure that all Scenes are added to the Build Settings Window
/// Ensure that Split Screen Setup scene will be the first scene in the list
/// </summary>

public class BuildSingleRuntimeWithOnePhysicalScreenContainsAllScreens : MonoBehaviour
{

    public static void BuildSplitScreen(BuildTarget PlatformBuildTarget)
    {
        // Creates a new BuildPlayerOptions
        var buildPlayerOptions = new BuildPlayerOptions
        {
            // Sets the scenes to be built in this build.
            // In this instance we are loading all scenes that are in the build settings windows. 
            // Alternate way to do this is creating an array with specific scenes you would want to load, 
            // Example: buildPlayerOptions.scenes = new[] { "Assets/Scenes/SplitScreen/SplitScreenScene_Setup.unity" };
            scenes = ReturnAllScenePathsInEditorBuildSettings(),
            
            // Sets the location of the build with folder structure and name of build and adds the extension for the selected platform
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
    /// </summary>
    private static string[] ReturnAllScenePathsInEditorBuildSettings()
    {
        var Paths = new List<string>();
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            Paths.Add(EditorBuildSettings.scenes[i].path);
        }

        return Paths.ToArray();
    }

    /// <summary>
    /// Location of build 
    /// </summary>
    public static string BuildLocationPath()
    {
        return "Builds/OneScreenAllScenes/SplitScreenBuild";
    }
}