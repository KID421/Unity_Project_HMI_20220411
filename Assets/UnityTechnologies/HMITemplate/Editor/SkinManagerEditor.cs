using HMI.UI.Skins;
using HMI.UI.Skins.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.UnityTechnologies.HMITemplate.Editor
{
    /// <summary>
    /// The skin manager editor supports cross-scene skin assignment
    /// </summary>
    public class SkinManagerEditor : EditorWindow
    {
        /// <summary>
        /// Default name of the skin swapper prefab
        /// </summary>
        private const string SkinSwapperName = "SkinSwapper.prefab";

        /// <summary>
        /// Shows the Skin Manager when selected from the Menu: HMI Framework > Skin Manager
        /// </summary>
        [MenuItem("HMI Framework / Skin Manager", false, 3)]
        public static void SkinManagerMenu()
        {
            GetWindow(typeof(SkinManagerEditor), false, "Skin Manager").Show();
        }

        /// <summary>
        /// Available skins
        /// </summary>
        private readonly List<UISkinCollectionData> Skins = new List<UISkinCollectionData>();

        /// <summary>
        /// Index in dropdown of the selected skin
        /// </summary>
        private int SelectedSkinIndex = -1;

        /// <summary>
        /// Selected skin
        /// </summary>
        private UISkinCollectionData SelectedSkin = null;

        /// <summary>
        /// Unity OnGUI callback
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
@"The Skin Manager switches between skins for all active scenes simultaneously.
Add the skinswapper Prefab to a scene where you want to swap skins.
You can add a new skin by duplicating a skin folder with a UISkinCollectionData in it 
and assigning it through the Skin Manager.", MessageType.Info);

            GUILayout.Space(10f);
            using var cHorizontalScope = new GUILayout.HorizontalScope();
            GUILayout.Space(20f);

            using var cVerticalScope = new GUILayout.VerticalScope();
            var swapperScripts = FindObjectsOfType<UISkinCollectionSwapper>();

            if (swapperScripts != null && SelectedSkin == null)
            {
                SelectedSkin = swapperScripts.First().SkinCollectionData;
            }

            SelectSkin();

            if (SelectedSkin != null)
            {
                if (GUILayout.Button("Select Skin in Project"))
                {
                    EditorGUIUtility.PingObject(SelectedSkin);
                }

                if (swapperScripts != null && swapperScripts.Length > 0)
                {
                    if (GUILayout.Button("Assign Skin to Scene"))
                    {
                        foreach (var swapperScript in swapperScripts)
                        {
                            if (swapperScript != null)
                            {
                                swapperScript.SetSkin(SelectedSkin);
                            }
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("No Skin Swapper Prefab found in scene, can't assig skin to scene.", MessageType.Warning);
                }

                if (GUILayout.Button("Select SkinSwapper Prefab in Project"))
                {
                    SelectSkinSwapperPrefabInProject();
                }
            }
        }

        /// <summary>
        /// Select SkinSwapper Prefab in Project
        /// </summary>
        private static void SelectSkinSwapperPrefabInProject()
        {
            var prefabs = AssetDatabase.FindAssets("t:Prefab");
            var found = false;

            foreach (var prefabID in prefabs)
            {
                var path = AssetDatabase.GUIDToAssetPath(prefabID);

                if (path.EndsWith(SkinSwapperName))
                {
                    var prefabGo = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    if (prefabGo != null)
                    {
                        var swapper = prefabGo.GetComponent<UISkinCollectionSwapper>();
                        EditorGUIUtility.PingObject(prefabGo);
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                EditorUtility.DisplayDialog("Could not locate SkinSwapper prefab.", "Could not locate SkinSwapper prefab.", "OK");
            }
        }

        /// <summary>
        /// Select a skin from the available skin dropdown
        /// </summary>
        private void SelectSkin()
        {

            GUILayout.Label("Available Skins", EditorStyles.boldLabel);
            var guids = AssetDatabase.FindAssets("t:UISkinCollectionData", null);

            Skins.Clear();
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                Skins.Add(AssetDatabase.LoadAssetAtPath<UISkinCollectionData>(path));
            }

            // match initial skin to skin used in scene
            if (SelectedSkin != null && SelectedSkinIndex == -1)
            {
                SelectedSkinIndex = Skins.IndexOf(SelectedSkin);
            }

            EditorGUI.BeginChangeCheck();
            var skinNames = Skins.Select(x => x.name).ToArray();
            SelectedSkinIndex = EditorGUILayout.Popup("Skins", SelectedSkinIndex, skinNames);

            if (EditorGUI.EndChangeCheck())
            {
                SelectedSkin = Skins[SelectedSkinIndex];
            }
        }
    }
}
