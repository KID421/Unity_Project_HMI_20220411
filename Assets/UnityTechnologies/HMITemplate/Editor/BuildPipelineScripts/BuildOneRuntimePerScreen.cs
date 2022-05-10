using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ensure that all Scenes are added to the Build Settings Window
/// </summary>

public class BuildOneRuntimePerScreen : MonoBehaviour
{
    /// <summary>
    /// Build one runtime per screen
    /// </summary>
    public static void BuildOneRuntimePerScreenBuild(BuildTarget PlatformBuildTarget)
    {
        // foreach scene in the EditorBuildSettings check if the scene is enabled
        // if scene is enabled, if the name of the scene is "SplitScreenSetup" or "MultiScreenSetup" then don't proceed otherwise continue
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            // Check if the scene is enabled
            if (EditorBuildSettings.scenes[i].enabled)
            {
                // If the name of the scene is "SplitScreenSetup" or "MultiScreenSetup" then don't proceed, otherwise continue    
                // SplitScreenSetup and MultiScreenSetup are specific scenes for setting up Split Screen and MultiScreen require their own 
                // unique build process for each of their setups.
                if (NameFromIndex(i) != "SplitScreenSetup" && NameFromIndex(i) != "MultiScreenSetup")
                {
                    // Creating the build, using the path of the current iteration of build scene: EditorBuildSettings.scenes[i].path, 
                    // and determining the target path and name of the build: Path + name from index + extension
                    var report = BuildPipeline.BuildPlayer(CreateBuildPlayerOptions(EditorBuildSettings.scenes[i].path, BuildLocationPath() + NameFromIndex(i) + BuildWindow_CustomEditor.GetExtensionForBuildTarget(PlatformBuildTarget), PlatformBuildTarget));

                    // Creates the build summary for checks
                    var summary = report.summary;

                    // If build is successful then we log the build size of the build in bytes.
                    if (summary.result == BuildResult.Succeeded)
                    {
                        Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                    }

                    // If build is unsuccessful then we log the build failure.
                    if (summary.result == BuildResult.Failed)
                    {
                        Debug.Log("Build failed");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates and returns the BuildPlayerOptions
    /// <param name="scenePathName"> the string path of the scene. </param>
    /// <param name="stringLocationPath"> the string location path of where the build will be located. </param>
    /// </summary>
    private static BuildPlayerOptions CreateBuildPlayerOptions(string scenePathName, string stringLocationPath, BuildTarget TargetPlatform)
    {
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { scenePathName },
            locationPathName = stringLocationPath,
            target = TargetPlatform,
            options = BuildOptions.None
        };

        return buildPlayerOptions;
    }

    /// <summary>
    ///  Returns the name of the scene without the path
    /// <param name="BuildIndex"> the int BuildIndex of the current scene. </param>
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
        return "Builds/OneRuntimePerScreen/";
    }
}
