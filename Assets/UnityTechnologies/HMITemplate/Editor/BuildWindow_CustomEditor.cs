using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom Build editor
/// </summary>
public class BuildWindow_CustomEditor : EditorWindow
{
    /// <summary>
    /// Shows the Custom Build Window menu when selected from the Menu: Build > Custom Build
    /// Opens the window at a custom rect for position and size
    /// </summary>
    [MenuItem("HMI Framework / Custom Build", false, 5)]
    public static void CustomBuildMenu()
    {
        var Window = GetWindow(typeof(BuildWindow_CustomEditor), false, "Custom Build Window");
        Window.position = new Rect(150, 150, 559, 333);
        Window.Show();
    }

    /// <summary>
    /// Array of strings that match the names of the build targets
    /// </summary>
    private readonly string[] BuildTargets = { "Windows", "Android", "macOS", "iOS", "Linux" };

    /// <summary>
    /// An index to track the dropdown selection
    /// </summary>
    private int BuildTargetIndex = 0;

    /// <summary>
    /// Texture representing the split screen icon
    /// </summary>
    private static Texture2D SplitScreenIcon = null;

    /// <summary>
    /// Texture representing the multi screen icon
    /// </summary>
    private static Texture2D MultiScreenIcon = null;

    /// <summary>
    /// Texture representing the runtime icon
    /// </summary>
    private static Texture2D RuntimeIcon = null;

    /// <summary>
    /// Bool representing if the split screen build has been selected or not
    /// </summary>
    private bool SplitScreenBuildChosen;

    /// <summary>
    /// Bool representing if the multi screen build has been selected or not
    /// </summary>
    private bool MultiScreenBuildChosen;

    /// <summary>
    /// Bool representing if the runtime build has been selected or not
    /// </summary>
    private bool OneRuntimePerSceneBuildChosen;

    /// <summary>
    /// List of the editor build settings scenes
    /// </summary>
    private readonly List<EditorBuildSettingsScene> AllCurrentEditorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

    /// <summary>
    /// On enable:
    /// Populate the editor build setting scenes
    /// Set the icons from the asset paths to the texture variables. 
    /// </summary>
    private void OnEnable()
    {
        PopulateEditorBuildSettingScenes();
        SplitScreenIcon = AssetDatabase.LoadAssetAtPath("Assets/UnityTechnologies/HMITemplate/Editor/EditorImages/SplitScreenIcon.png", typeof(Texture2D)) as Texture2D;
        MultiScreenIcon = AssetDatabase.LoadAssetAtPath("Assets/UnityTechnologies/HMITemplate/Editor/EditorImages/MultiScreenIcon.png", typeof(Texture2D)) as Texture2D;
        RuntimeIcon = AssetDatabase.LoadAssetAtPath("Assets/UnityTechnologies/HMITemplate/Editor/EditorImages/RuntimeIcon.png", typeof(Texture2D)) as Texture2D;
    }

    /// <summary>
    /// Unity OnGui callback
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Label("Custom Build Menu", EditorStyles.boldLabel);
        GUILayout.Label("Build Options:", EditorStyles.label);
        GUILayout.Space(2);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // Create toggle using the icon of multiscreen at a max height and size.
        MultiScreenBuildChosen = GUILayout.Toggle(MultiScreenBuildChosen, MultiScreenIcon, GUILayout.MaxWidth(160), GUILayout.MaxHeight(128));
        
        // Create a flexible space to help center the objects.
        GUILayout.FlexibleSpace();

        // Create toggle using the icon of splitscreen at a max height and size.
        SplitScreenBuildChosen = GUILayout.Toggle(SplitScreenBuildChosen, SplitScreenIcon, GUILayout.MaxWidth(160), GUILayout.MaxHeight(128));
        GUILayout.FlexibleSpace();
       
        // Create toggle using the icon of runtimebuild at a max height and size.
        OneRuntimePerSceneBuildChosen = GUILayout.Toggle(OneRuntimePerSceneBuildChosen, RuntimeIcon, GUILayout.MaxWidth(160), GUILayout.MaxHeight(128));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Creates an area to house the Build Target label and the Build Target index popup that houses the build targets
        GUILayout.BeginArea(new Rect(10, 175, Screen.width, 50));
        GUILayout.Label("Build Target: ", EditorStyles.label);

        // Places popup in the area defined, based on the screen position divided by 
        // 2 multiplied by our control dpi divided by the screen dpi - 13 to compensate for side buffer
        // Creates width based on control screen dpi divided by current screen dpi times the half screen width
        BuildTargetIndex = EditorGUI.Popup(new Rect(
            (Screen.width / 2) * (96 / Screen.dpi) - 13, 
            0, 
            (Screen.width / 2) * (96 / Screen.dpi), 20), 
            BuildTargetIndex, BuildTargets);

        GUILayout.EndArea();
        GUILayout.Space(20);

        // Creates a button that runs the builds selected when pressed.
        if (GUILayout.Button("Build"))
        {
            RunBuildTargets();
        }

        GUILayout.Space(10);

        // If any of the toggles for the builds are chosen, then show the label
        if (MultiScreenBuildChosen || SplitScreenBuildChosen || OneRuntimePerSceneBuildChosen)
        {
            GUILayout.Label("Build Locations: ", EditorStyles.boldLabel);
        }

        // If the toggle is on, then show the build location
        if (MultiScreenBuildChosen)
        {
            GUILayout.Label("MultiScreen Build Location: " + BuildSingleRuntimeWithOneScreenPerScreen.BuildLocationPath());
        }

        // If the toggle is on, then show the build location
        if (SplitScreenBuildChosen)
        {
            GUILayout.Label("Split Screen Build Location: " + BuildSingleRuntimeWithOnePhysicalScreenContainsAllScreens.BuildLocationPath());
        }

        // If the toggle is on, then show the build location
        if (OneRuntimePerSceneBuildChosen)
        {
            GUILayout.Label("One Runtime Per Build Location: " + BuildOneRuntimePerScreen.BuildLocationPath());
        }
    }

    /// <summary>
    /// For each of the selected build types, will execute that build to the desired build target
    /// </summary>
    private void RunBuildTargets()
    {
        if (SplitScreenBuildChosen)
        {
            BuildSingleRuntimeWithOnePhysicalScreenContainsAllScreens.BuildSplitScreen(ReturnDesiredBuildTarget());
        }
        if (MultiScreenBuildChosen)
        {
            BuildSingleRuntimeWithOneScreenPerScreen.BuildMultiScreen(ReturnDesiredBuildTarget());
        }
        if (OneRuntimePerSceneBuildChosen)
        {
            BuildOneRuntimePerScreen.BuildOneRuntimePerScreenBuild(ReturnDesiredBuildTarget());
        }
    }

    /// <summary>
    /// Returns the desired build target selected in the dropdown of the menu
    /// </summary>
    /// <returns></returns>
    private BuildTarget ReturnDesiredBuildTarget()
    {
        return BuildTargets[BuildTargetIndex] switch
        {
            "Windows" => BuildTarget.StandaloneWindows,
            "Android" => BuildTarget.Android,
            "macOS" => BuildTarget.StandaloneOSX,
            "iOS" => BuildTarget.iOS,
            "Linux" => BuildTarget.StandaloneLinux64,
            _ => BuildTarget.StandaloneWindows,
        };
    }

    /// <summary>
    /// Populates the list of AllCurrentEditorBuildSettingsScenes with all scenes added in the EditorBuildSettings.scenes
    /// </summary>
    private void PopulateEditorBuildSettingScenes()
    {
        AllCurrentEditorBuildSettingsScenes.Clear();

        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            AllCurrentEditorBuildSettingsScenes.Add(EditorBuildSettings.scenes[i]);
        }
    }

    /// <summary>
    /// Find valid Scene paths and make a list of EditorBuildSettingsScene
    /// Set the Build Settings window Scene list
    /// </summary>
    public void SetEditorBuildSettingsScenes()
    {
        var EditorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

        foreach (var scene in EditorBuildSettings.scenes)
        {
            var scenePath = scene.path;
            if (!string.IsNullOrEmpty(scenePath))
                EditorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        }

        EditorBuildSettings.scenes = EditorBuildSettingsScenes.ToArray();
    }
    /// <summary>
    /// A helper function that returns the extension required for the target platform
    /// </summary>
    public static string GetExtensionForBuildTarget(BuildTarget target)
    {
        return target switch
        {
            BuildTarget.StandaloneWindows => ".exe",
            BuildTarget.Android => ".apk",
            BuildTarget.StandaloneOSX => "",
            BuildTarget.iOS => "",
            BuildTarget.StandaloneLinux64 => "",
            _ => "",
        };
    }
}
