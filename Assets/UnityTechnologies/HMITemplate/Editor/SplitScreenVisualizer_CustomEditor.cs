using MultiScreenFramework.SplitScreenSetup;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Creates a custom editor on the SplitScreenVisualizerScript
/// Can edit Multiple Objects
/// </summary>
[CustomEditor(typeof(SplitScreenVisualizerScript))]
[CanEditMultipleObjects]
public class SplitScreenVisualizer_CustomEditor : Editor
{
    /// <summary>
    /// Gets the serialized property Config
    /// </summary>
    private SerializedProperty Config;

    /// <summary>
    /// Variable to track the split screen setup configuration
    /// </summary>
    private SplitScreenSetupConfiguration SplitScreenSetup;

    /// <summary>
    /// Variable to set the Buffer that the visualizer will have from the edge of the inspector window
    /// </summary>
    private readonly float EdgeBuffer = 16;

    /// <summary>
    /// On Enable, set the Config variable to finding the property SplitScreenScript
    /// On Enable, find the SplitScreenSetupConfiguration in the scene and set our variable
    /// </summary>
    private void OnEnable()
    {
        Config = serializedObject.FindProperty("SplitScreenScript");
        SplitScreenSetup = FindObjectOfType<SplitScreenSetupConfiguration>();
    }

    /// <summary>
    /// Unity OnInspectorGUI callback
    /// </summary>
    public override void OnInspectorGUI()
    {
        var areaHeight = 425f;
        var DisplayData = SplitScreenSetup.DisplayData;

        serializedObject.Update();
        EditorGUILayout.PropertyField(Config);
        serializedObject.ApplyModifiedProperties();
        GUILayout.BeginArea(new Rect(0, 50, Screen.width, areaHeight));

        if (DisplayData != null)
        {
            var backgroundWidth = ReturnDrawAreaWidth(DisplayData, ReturnDrawAreaHeight(DisplayData));
            var backgroundHeight = ReturnDrawAreaHeightWithBuffer(DisplayData);
            var backgroundColor = new Color(1, 1, 1, .1f);
            var startingPointX = (Screen.width / EdgeBuffer);
            var startingPointY = 0f;

            // Background Rect drawn to correct aspect ratio based on Display Data
            DrawRect(startingPointX, startingPointY, backgroundWidth - (backgroundWidth / (EdgeBuffer / 2)), backgroundHeight, backgroundColor);

            // Searched all SceneList items and sets up a visual representation of the screen and rect in our visualizer
            for (var i = 0; i < SplitScreenSetup.ScenesList.Count; i++)
            {
                var CamRect = ReturnCameraRect(SplitScreenSetup.ScenesList[i], startingPointX, startingPointY, backgroundWidth, backgroundHeight);
                DrawRect(CamRect.x, CamRect.y, CamRect.width, CamRect.height, ReturnColor(i));
            }

            // Draws an aesthetic outline around our visualizer
            DrawRect(startingPointX, startingPointY, backgroundWidth - (backgroundWidth / (EdgeBuffer / 2)), 2, Color.black);
            DrawRect(startingPointX, backgroundHeight, backgroundWidth - (backgroundWidth / (EdgeBuffer / 2)), 2, Color.black);
            DrawRect(startingPointX, startingPointY, 2, backgroundHeight, Color.black);
            DrawRect(backgroundWidth - (backgroundWidth / EdgeBuffer), startingPointY, 2, backgroundHeight, Color.black);
        }

        // Ends our Background Area
        GUILayout.EndArea();

        // Adjusts the height of the script window in the editor
        GUILayout.Space(425);

    }

    /// <summary>
    /// Draws the rect in the Gui and sets the color
    /// </summary>
    /// <param name="x"> The X value </param>
    /// <param name="y"> The Y value </param>
    /// <param name="w"> The Width Value </param>
    /// <param name="h"> The Height Value </param>
    /// <param name="c"> The Color Value </param>
    private void DrawRect(float x, float y, float w, float h, Color c)
    {
        var NewRect = new Rect(x, y, w, h);
        EditorGUI.DrawRect(NewRect, c);
    }

    /// <summary>
    /// Returns the Draw Area Width
    /// </summary>
    /// <param name="DisplayData"> The display data from the scriptable object </param>
    /// <param name="DrawnRectHeight"> The drawn rect height </param>
    /// <returns></returns>
    private float ReturnDrawAreaWidth(ScreenSetup_ScriptableObject DisplayData, float DrawnRectHeight)
    {
        return (DrawnRectHeight * DisplayData.DisplayData.Screen_WidthResolution) / DisplayData.DisplayData.Screen_HeightResolution;
    }

    /// <summary>
    /// Returns the Draw Area Height
    /// </summary>
    /// <param name="DisplayData"> The display data from the scriptable object </param>
    /// <returns></returns>
    private float ReturnDrawAreaHeight(ScreenSetup_ScriptableObject DisplayData)
    {
        return (Screen.width * DisplayData.DisplayData.Screen_HeightResolution) / DisplayData.DisplayData.Screen_WidthResolution;
    }

    /// <summary>
    /// Returns the draw area with the included buffer from the sides of the screen.
    /// </summary>
    /// <param name="DisplayData"> The display data from the scriptable object </param>
    /// <returns></returns>
    private float ReturnDrawAreaHeightWithBuffer(ScreenSetup_ScriptableObject DisplayData)
    {
        return ((ReturnDrawAreaWidth(DisplayData, ReturnDrawAreaHeight(DisplayData)) - 
            (ReturnDrawAreaWidth(DisplayData, ReturnDrawAreaHeight(DisplayData)) / 8)) * 
            DisplayData.DisplayData.Screen_HeightResolution) / DisplayData.DisplayData.Screen_WidthResolution;
    }

    /// <summary>
    /// Returns the Camera Rect from the script
    /// </summary>
    /// <param name="SetupClass"> The class that contains the information  about the setup</param>
    /// <param name="StartingPointX"> Starting point X of the Background Area</param>
    /// <param name="StartingPointY"> Starting point Y of the Background Area</param>
    /// <param name="backgroundWidth"> The background width </param>
    /// <param name="backgroundHeight"> The background height </param>
    private Rect ReturnCameraRect(SplitScreenSetupConfiguration.SplitScreenSetupClass SetupClass,
        float StartingPointX, 
        float StartingPointY, 
        float backgroundWidth, 
        float backgroundHeight)
    {
        var x = SetupClass.CameraViewport_X;
        var y = 1 - ((1 - SetupClass.CameraViewport_Y) - SetupClass.CameraViewport_H);
        var h = SetupClass.CameraViewport_H;
        var w = SetupClass.CameraViewport_W;
        var newH = (h * backgroundHeight);
        var newW = (w * (backgroundWidth - (backgroundWidth / (EdgeBuffer / 2))));
        var newX = (x * (backgroundWidth - (backgroundWidth / (EdgeBuffer / 2)))) + StartingPointX;
        var newY = backgroundHeight - (y * backgroundHeight);

        return new Rect(newX, newY, newW, newH);
    }

    /// <summary>
    /// Returns a color based on what iteration you input
    /// </summary>
    /// <param name="i"> the iteration of color you would like returned</param>
    /// <returns> returned the color </returns>
    private Color ReturnColor(int i)
    {
        return i switch
        {
            0 => new Color(1, 0, 0, .5f),
            1 => new Color(0, 1, 0, .5f),
            2 => new Color(0, 0, 1, .5f),
            3 => new Color(0, 1, 1, .5f),
            4 => new Color(1, 1, 0, .5f),
            5 => new Color(1, 0, 1, .5f),
            _ => new Color(1, 1, 1, .5f),
        };
    }
}
