using UnityEditor;
using UnityEngine;

public class ControlsSetup_Editor : EditorWindow
{
    /// <summary>
    /// Shows the Controls Setup Services Menu when selected from the Menu: HMI Framework > Controls Setup Services
    /// </summary>
    [MenuItem("HMI Framework / Controls Setup Services", false, 3)]
    public static void ControlsServicesMenu()
    {
        var Window = GetWindow(typeof(ControlsSetup_Editor), false, "Controls Key Mapping");
        Window.Show();
    }

    /// <summary>
    /// the data object that will be modified in this script
    /// </summary>
    private Controls_ScriptableObject ControlsScriptableObject = null;

    /// <summary>
    /// a bool for checking if we should map keys based on next unity event
    /// </summary>
    private bool KeyMapLive = false;

    /// <summary>
    /// a string for keeping track of the name of the key intended to remap.
    /// </summary>
    private string NameOfSelectedKey = null;

    /// <summary>
    /// Scroll support
    /// </summary>
    private Vector2 ScrollArea;

    /// <summary>
    /// On first time setup, the ControlsScriptableObject is loaded
    /// </summary>
    private bool FirstTimeSetupComplete = false;

    /// <summary>
    /// Unity OnGUI Callback
    /// </summary>
    private void OnGUI()
    {
        // The exposed ControlsScriptableObject pop up selector in the editor window.
        GUILayout.Label("Please assign Controls Data to the selection.", EditorStyles.boldLabel);
        var tmp = (Controls_ScriptableObject)EditorGUILayout.ObjectField(ControlsScriptableObject, typeof(Controls_ScriptableObject), false);

        // Check if the selected tmp ControlsScriptableObject matches the current ControlsScriptableObject
        // if it doesnt match, then check if tmp is null, if null then clear()
        // otherwise set ControlsScriptableObject = tmp;
        // If this is first time setup, then attempt to find the data asset and set it
        if (tmp != ControlsScriptableObject)
        {
            if (tmp == null)
            {
                Clear();

            }
            else
            {
                ControlsScriptableObject = tmp;
                Reconnect();
            }
        }
        else if (tmp == null)
        {
            if (!FirstTimeSetupComplete)
            {
                AttemptToFindDataAsset();
                FirstTimeSetupComplete = true;
            }
        }

        if (ControlsScriptableObject != null)
        {
            // If we start our keymapping through KeyMapLive
            // Look for if the event is a key press
            // if so, then search through ControlsScriptableObject.ControlsMappingList to match the ControlsScriptableObject.ControlsMappingList[i].Name to the NameOfSelectedKey
            // Then assign the KeyCode of ControlsScriptableObject.ControlsMappingList[i].AssignedKey to the event key pressed
            // Then clear both the KeyMapLive and NameOfSelectedKey. 
            if (KeyMapLive)
            {
                var e = Event.current;

                if (e.isKey)
                {
                    Debug.Log("New key mapped is: " + e.keyCode.ToString());

                    for (var i = 0; i < ControlsScriptableObject.ControlsMappingList.Count; i++)
                    {
                        if (ControlsScriptableObject.ControlsMappingList[i].Name == NameOfSelectedKey)
                        {
                            ControlsScriptableObject.ControlsMappingList[i].AssignedKey = e.keyCode;
                        }
                    }

                    KeyMapLive = false;
                    NameOfSelectedKey = null;
                }
            }

            // For each of the ControlsMappingList in ControlsScriptableObject We will generate a label and button side by side.
            // If button is currently being mapped, then we will change the name of the button to "Press new key to map button"
            // If that button is selected then we will trigger Keymapping through KeyMapLive = true;
            ScrollArea = GUILayout.BeginScrollView(ScrollArea, GUIStyle.none);

            for (var i = 0; i < ControlsScriptableObject.ControlsMappingList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(ControlsScriptableObject.ControlsMappingList[i].Name, EditorStyles.label);
                GUILayout.FlexibleSpace();

                if (ControlsScriptableObject.ControlsMappingList[i].Name == NameOfSelectedKey)
                {
                    GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("Press new key to map button", GUILayout.Width(200)))
                    {
                        NameOfSelectedKey = ControlsScriptableObject.ControlsMappingList[i].Name;
                        KeyMapLive = true;
                    }
                }
                else
                {
                    GUI.backgroundColor = Color.white;
                    if (GUILayout.Button(ControlsScriptableObject.ControlsMappingList[i].AssignedKey.ToString(), GUILayout.Width(200)))
                    {
                        Debug.Log("Starting Mapping Process. Press desired Key to complete mapping.");
                        NameOfSelectedKey = ControlsScriptableObject.ControlsMappingList[i].Name;
                        KeyMapLive = true;

                    }
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }
    }

    /// <summary>
    /// Clearing the variables when no ControlsScriptableObject is selected.
    /// </summary>
    private void Clear()
    {
        ControlsScriptableObject = null;
        Debug.Log("Clearing");
    }

    /// <summary>
    /// Taking our variables and connecting them to the scripts on the children
    /// of the source electric vehicle. 
    /// </summary>
    private void Reconnect()
    {
        Debug.Log("Reconnecting");
    }

    /// <summary>
    /// Attempts to find the data asset in the scene
    /// If found, then it sets the data asset to the first in the list
    /// </summary>
    private void AttemptToFindDataAsset()
    {
        Debug.Log("Setting data asset");
        var AllControlAssets = AssetDatabase.FindAssets("t:Controls_ScriptableObject");
        if (AllControlAssets.Length > 0)
        {
            ControlsScriptableObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AllControlAssets[0]), typeof(Controls_ScriptableObject)) as Controls_ScriptableObject;
        }
    }
}