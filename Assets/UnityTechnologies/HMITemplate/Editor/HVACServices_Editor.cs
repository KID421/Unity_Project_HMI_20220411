using HMI.Vehicles.Services;
using UnityEditor;
using UnityEngine;

/// <summary>
/// HVAC Editor
/// </summary>
public class HVACServices_Editor : EditorWindow
{
    /// <summary>
    /// Shows the HVAC Services Menu when selected from the Menu: HMI Framework > HVAC Services
    /// </summary>
    [MenuItem("HMI Framework / Simulation Services / HVAC Services", false, 2)]
    public static void HVACServicesMenu()
    {
        GetWindow(typeof(HVACServices_Editor), false, "HVAC Services").Show();
    }

    /// <summary>
    /// Unity OnGUi callback
    /// </summary>
    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            var HvacScript = HVACService.GetHVAC();

            // Checking if the Hvac Script is not null
            // If not null, then create variables that are linked to scripts for interaction
            if (HvacScript != null)
            {
                // Label for Left Climate Zone Temperature of the user interface in the editor window.
                GUILayout.Label("Left Climate Zone Temperature (celsius)", EditorStyles.boldLabel);

                // Creates a slider in the editor window that is connected to the LeftClimateZoneTemperature on the HvacScript.
                // When this slider is moved, it changes the values of LeftClimateZoneTemperature for HvacScript.
                HvacScript.GetClimateZone("LeftClimateZone").Temperature =
                    EditorGUILayout.Slider(HvacScript.GetClimateZone("LeftClimateZone").Temperature,
                    HvacScript.GetClimateZone("LeftClimateZone").MinTemperature,
                    HvacScript.GetClimateZone("LeftClimateZone").MaxTemperature);

                // Label for Right Climate Zone Temperature of the user interface in the editor window.
                GUILayout.Label("Right Climate Zone Temperature (celsius)", EditorStyles.boldLabel);

                // Creates a slider in the editor window that is connected to the RightClimateZoneTemperature on the HvacScript.
                // When this slider is moved, it changes the values of RightClimateZoneTemperature for HvacScript.
                HvacScript.GetClimateZone("RightClimateZone").Temperature =
                    EditorGUILayout.Slider(HvacScript.GetClimateZone("RightClimateZone").Temperature,
                    HvacScript.GetClimateZone("RightClimateZone").MinTemperature,
                    HvacScript.GetClimateZone("RightClimateZone").MaxTemperature);

                // Label for Right Climate Zone Fan Speeds of the user interface in the editor window.
                GUILayout.Label("Fan Speed - Right Climate Zone", EditorStyles.boldLabel);

                // Creates a slider in the editor window that is connected to the FanSpeed_RightClimateZone on the HvacScript.
                // When this slider is moved, it changes the values of FanSpeed_RightClimateZone for HvacScript.
                var blower = HvacScript.GetBlower("PassengerSeatBlower");
                blower.CurrentLevel =
                    EditorGUILayout.IntSlider(blower.CurrentLevel,
                    0,
                    blower.Levels);

                // Label for Left Climate Zone Fan Speeds of the user interface in the editor window.
                GUILayout.Label("Fan Speed - Left Climate Zone", EditorStyles.boldLabel);

                // Creates a slider in the editor window that is connected to the FanSpeed_LeftClimateZone on the HvacScript.
                // When this slider is moved, it changes the values of FanSpeed_LeftClimateZone for HvacScript.
                blower = HvacScript.GetBlower("DriverSeatBlower");
                blower.CurrentLevel =
                    EditorGUILayout.IntSlider(blower.CurrentLevel,
                    0,
                    blower.Levels);

                GUILayout.Label("Seat Heater - Left Climate Zone", EditorStyles.boldLabel);

                // Creates a slider in the editor window that is connected to the SeatHeaterLevel_LeftClimateZone on the HvacScript.
                var heating = HvacScript.GetHeatingSystem("DriverSeat");
                heating.CurrentLevel =
                    EditorGUILayout.IntSlider(
                        heating.CurrentLevel,
                        0,
                        heating.Levels);

                GUILayout.Label("Seat Heater - Right Climate Zone", EditorStyles.boldLabel);
                // Creates a slider in the editor window that is connected to the SeatHeaterLevel_RightClimateZone on the HvacScript.
                // When this slider is moved, it changes the values of SeatHeaterLevel_RightClimateZone for HvacScript.
                heating = HvacScript.GetHeatingSystem("PassengerSeat");
                heating.CurrentLevel =
                    EditorGUILayout.IntSlider(
                        heating.CurrentLevel,
                        0,
                        heating.Levels);

                // Label for Toggles of the user interface in the editor window.
                GUILayout.Label("Toggles", EditorStyles.boldLabel);

                // Creates a bool in the editor window that is connected to the IsAirConditioningActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsAirConditioningActivated.
                HvacScript.IsAirConditioningActivated = EditorGUILayout.Toggle("AC Toggle", HvacScript.IsAirConditioningActivated);

                // Creates a bool in the editor window that is connected to the IsMaxAirConditioningActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsMaxAirConditioningActivated.
                HvacScript.IsMaxAirConditioningActivated = EditorGUILayout.Toggle("Max AC Toggle", HvacScript.IsMaxAirConditioningActivated);

                // Creates a bool in the editor window that is connected to the IsAutoTempControlActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsAutoTempControlActivated.
                HvacScript.IsAutoTempControlActivated = EditorGUILayout.Toggle("Auto Toggle", HvacScript.IsAutoTempControlActivated);

                // Creates a bool in the editor window that is connected to the IsLeftSeatVentilationActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsLeftSeatVentilationActivated.
                HvacScript.IsLeftSeatVentilationActivated = EditorGUILayout.Toggle("Left Seat Ventilation Toggle", HvacScript.IsLeftSeatVentilationActivated);

                // Creates a bool in the editor window that is connected to the IsRightSeatVentilationActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsRightSeatVentilationActivated.
                HvacScript.IsRightSeatVentilationActivated = EditorGUILayout.Toggle("Right Seat Ventilation Toggle", HvacScript.IsRightSeatVentilationActivated);

                // Creates a bool in the editor window that is connected to the IsRecirculationActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsRecirculationActivated.
                HvacScript.IsRecirculationActivated = EditorGUILayout.Toggle("Recirculate Air Toggle", HvacScript.IsRecirculationActivated);

                // Creates a bool in the editor window that is connected to the IsAirInActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsAirInActivated.
                HvacScript.IsAirInActivated = EditorGUILayout.Toggle("Air In Toggle", HvacScript.IsAirInActivated);

                // Creates a bool in the editor window that is connected to the IsSyncButtonActivated on the HvacScript.
                // When this bool is selected, it will turn on / off the bool on HvacScript for IsSyncButtonActivated.
                HvacScript.IsSyncButtonActivated = EditorGUILayout.Toggle("Sync Button Toggle", HvacScript.IsSyncButtonActivated);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Please put editor in play mode to interact with HVAC services", MessageType.Info);
        }
    }
}