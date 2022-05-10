using HMI.ControlsManager;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor that is the type of ControlsManagerScript
/// </summary>
[CustomEditor(typeof(ControlsManagerScript))]
public class ControlsManagerScript_CustomEditor : Editor
{
    /// <summary>
    /// ControlsData assignment for the controls manager
    /// </summary>
    private SerializedProperty DataScript;

    /// <summary>
    /// List of Keypresses of the controls manager
    /// </summary>
    private SerializedProperty KeyPressEventsList;

    /// <summary>
    /// On awake, sets the script to the target ControlsManagerScript
    /// Calls to updaet all controls list on the script
    /// </summary>
    private void Awake()
    {
        var Script = (ControlsManagerScript)target;
        Script.UpdateAllControlsList();
    }

    /// <summary>
    /// Unity OnEnable callback
    /// </summary>
    private void OnEnable()
    {
        DataScript = serializedObject.FindProperty("ControlsData");
        KeyPressEventsList = serializedObject.FindProperty("KeyPressEvents");
    }

    /// <summary>
    /// Draws the base inspector
    /// Sets the target to the ControlsManagerScript target
    /// Creates a button UpdateControlMapping that will forcibly update all controls on the list
    /// </summary>
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(DataScript);
        GUILayout.Space(10);
        var Script = (ControlsManagerScript)target;

        if (GUILayout.Button("Refresh Key Press Events"))
        {
            Script.UpdateAllControlsList();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(KeyPressEventsList);
        if (EditorGUI.EndChangeCheck())
        {
            Script.UpdateAllControlsList();
        }
    }
}