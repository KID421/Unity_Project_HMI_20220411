using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HMI.UI
{
    /// <summary>
    /// Custom editor for the sprite group color alpha script
    /// </summary>
    [CustomEditor(typeof(SpriteGroupColorAlpha))]
    public class SpriteGroupColorAlphaEditor : Editor
    {
        /// <summary>
        /// On Inspector GUI callback
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var spriteGroupColorAlpha = (SpriteGroupColorAlpha)target;

            if (GUILayout.Button("Load Sprite Renderers"))
            {
                var pairs = new List<SpriteRendererColorPair>();
                var renderers = spriteGroupColorAlpha.GetComponentsInChildren<SpriteRenderer>();

                foreach (var renderer in renderers)
                {
                    pairs.Add(new SpriteRendererColorPair()
                    {
                        Renderer = renderer,
                        Color = renderer.color
                    });
                }

                spriteGroupColorAlpha.SpriteColorPairs = pairs;
            }
        }
    }
}
