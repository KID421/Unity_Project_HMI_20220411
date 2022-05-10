using UnityEditor;
using UnityEngine;
using HMI.UI.IVI;

/// <summary>
/// Custom inspector for the screen orientation helper
/// </summary>
[CustomEditor(typeof(ScreenOrientationHelper))]
public class ScreenOrientationHelper_CustomInpector : Editor
{
    /// <summary>
    /// Inspector GUI callback
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        var Script = (ScreenOrientationHelper)target;

        if (Script.LandscapeSettings != null)
        {
            for (var i = 0; i < Script.LandscapeSettings.Length; i++)
            {
                if (Script.LandscapeSettings[i].GameObject != null && Script.LandscapeSettings[i].Rect == null)
                {
                    Script.LandscapeSettings[i].Rect = Script.LandscapeSettings[i].GameObject.GetComponent<RectTransform>();

                    //Position
                    Script.LandscapeSettings[i].Position = Script.LandscapeSettings[i].Rect.anchoredPosition;

                    //Size
                    Script.LandscapeSettings[i].Size = Script.LandscapeSettings[i].Rect.sizeDelta;

                    //Anchors
                    Script.LandscapeSettings[i].AnchorMax = Script.LandscapeSettings[i].Rect.anchorMax;
                    Script.LandscapeSettings[i].AnchorMin = Script.LandscapeSettings[i].Rect.anchorMin;

                    //Pivot
                    Script.LandscapeSettings[i].Pivot = Script.LandscapeSettings[i].Rect.pivot;

                    //rotation
                    Script.LandscapeSettings[i].Rotation = Script.LandscapeSettings[i].Rect.localEulerAngles;

                    //Size
                    Script.LandscapeSettings[i].Scale = Script.LandscapeSettings[i].Rect.localScale;
                }

                if (Script.LandscapeSettings[i].GameObject == null && Script.LandscapeSettings[i].Rect != null)
                {
                    Script.LandscapeSettings[i] = new ScreenOrientationHelper.CanvasObject();
                }
            }
        }

        if (Script.PortraitSettings != null)
        {
            for (var i = 0; i < Script.PortraitSettings.Length; i++)
            {
                if (Script.PortraitSettings[i].GameObject != null && Script.PortraitSettings[i].Rect == null)
                {
                    Script.PortraitSettings[i].Rect = Script.PortraitSettings[i].GameObject.GetComponent<RectTransform>();

                    //Position
                    Script.PortraitSettings[i].Position = Script.PortraitSettings[i].Rect.anchoredPosition;

                    //Size
                    Script.PortraitSettings[i].Size = Script.PortraitSettings[i].Rect.sizeDelta;

                    //Anchors
                    Script.PortraitSettings[i].AnchorMax = Script.PortraitSettings[i].Rect.anchorMax;
                    Script.PortraitSettings[i].AnchorMin = Script.PortraitSettings[i].Rect.anchorMin;

                    //Pivot
                    Script.PortraitSettings[i].Pivot = Script.PortraitSettings[i].Rect.pivot;

                    //rotation
                    Script.PortraitSettings[i].Rotation = Script.PortraitSettings[i].Rect.localEulerAngles;

                    //Size
                    Script.PortraitSettings[i].Scale = Script.PortraitSettings[i].Rect.localScale;
                }

                if (Script.PortraitSettings[i].GameObject == null && Script.PortraitSettings[i].Rect != null)
                {
                    Script.PortraitSettings[i] = new ScreenOrientationHelper.CanvasObject();
                }
            }
        }
    }
}
