using HMI.Vehicles.Behaviours;
using HMI.Vehicles.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using MultiScreenFramework.SplitScreenSetup;

/// <summary>
/// Car Services editor window
/// </summary>
public class CarServices_Editor : EditorWindow
{
    /// <summary>
    /// Shows the Car services Menu when selected from the Menu: HMI Framework > Mock Services
    /// </summary>
    [MenuItem("HMI Framework / Simulation Services / Car Services", false, 1)]
    public static void CarServicesMenu()
    {
        GetWindow(typeof(CarServices_Editor), false, "Car Services").Show();
    }

    /// <summary>
    /// The list index for the dropdown menu for PRNDL
    /// </summary>
    private int ListIdx = 0;

    /// <summary>
    /// The previous list index for checking change in the dropdown menu for PRNDL
    /// </summary>
    private int PrevListIdx = 0;

    /// <summary>
    /// The list options for the dropdown menu for PRNDL
    /// </summary>
    public List<string> MyList = new List<string>(new string[] { "Park", "Reverse", "Neutral", "Drive" });

    /// <summary>
    /// Split Screen script for checking loading
    /// </summary>
    SplitScreenSetupConfiguration SplitScreenScript;


    /// <summary>
    /// Unity OnGUI callback
    /// </summary>
    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            if (CheckIfSetupIsCompleted())
            {
                var vehicle = VehicleService.GetVehicle();

                if (vehicle != null)
                {
                    GUILayout.Label("Vehicle Speed", EditorStyles.boldLabel);
                    vehicle.CurrentSpeed = EditorGUILayout.Slider(vehicle.CurrentSpeed, 0, 180);
                }

                // Label for Telltales section of the user interface in the editor window.
                var carSystemsScript = FindObjectsOfType<CarSystems>().FirstOrDefault();

                // Checking if the Parking Brake Script is not null
                // If not null, then create variables that are linked to scripts for interaction
                if (carSystemsScript != null)
                {
                    GUILayout.Label("Telltales", EditorStyles.boldLabel);

                    // Creates a bool in the editor window that is connected to the IsParkingBrakeOn on the ParkingBrakeScript.
                    // When this bool is selected, it will turn on / off the bool on ParkingBrakeScript for IsParkingBrakeOn.
                    carSystemsScript.Data.IsParkingBrakeOn = EditorGUILayout.Toggle("Parking Brake Toggle", carSystemsScript.Data.IsParkingBrakeOn);

                    // Creates a bool in the editor window that is connected to the IsParkingBrakeMalfunctioning on the ParkingBrakeScript.
                    // When this bool is selected, it will turn on / off the bool on ParkingBrakeScript for IsParkingBrakeMalfunctioning.
                    carSystemsScript.Data.IsParkingBrakeMalfunctioning = EditorGUILayout.Toggle("Parking Brake Malfunctioning Toggle", carSystemsScript.Data.IsParkingBrakeMalfunctioning);

                    // Creates a bool in the editor window that is connected to the IsTirePressureLow on the TiresScript.
                    // When this bool is selected, it will turn on / off the bool on TiresScript for IsTirePressureLow.
                    carSystemsScript.Data.IsTirePressureLow = EditorGUILayout.Toggle("Tire Pressure Low Toggle", carSystemsScript.Data.IsTirePressureLow);

                    //Creates a bool in the editor window that is connected to the IsAnySeatbeltNotFastened on the SeatbeltsScript.
                    /// When this bool is selected, it will turn on / off the bool on SeatbeltsScript for IsAnySeatbeltNotFastened.
                    carSystemsScript.Data.IsAnySeatbeltNotFastened = EditorGUILayout.Toggle("Any Seatbelt Not Fashioned Toggle", carSystemsScript.Data.IsAnySeatbeltNotFastened);

                    // Creates a bool in the editor window that is connected to the IsABSMalfunctioning on the AbsScript.
                    // When this bool is selected, it will turn on / off the bool on AbsScript for IsABSMalfunctioning.
                    carSystemsScript.Data.IsABSMalfunctioning = EditorGUILayout.Toggle("ABS Malfunctioning Toggle", carSystemsScript.Data.IsABSMalfunctioning);

                    // Creates a bool in the editor window that is connected to the IsESCActive on the AbsScript.
                    // When this bool is selected, it will turn on / off the bool on AbsScript for IsESCActive.
                    carSystemsScript.Data.IsESCActive = EditorGUILayout.Toggle("ESC Active Toggle", carSystemsScript.Data.IsESCActive);

                    // Creates a bool in the editor window that is connected to the AirbagsActivated on the AirBagsScript.
                    // When this bool is selected, it will turn on / off the bool on AirBagsScript for AirbagsActivated.
                    carSystemsScript.Data.AreAirbagsActivated = EditorGUILayout.Toggle("Airbags Activated Toggle", carSystemsScript.Data.AreAirbagsActivated);

                    // Creates a bool in the editor window that is connected to the IsAnyDoorOrTrunkOpen on the DoorsScript.
                    // When this bool is selected, it will turn on / off the bool on DoorsScript for IsAnyDoorOrTrunkOpen.
                    carSystemsScript.Data.IsAnyDoorOrTrunkOpen = EditorGUILayout.Toggle("Door or Trunk Open Toggle", carSystemsScript.Data.IsAnyDoorOrTrunkOpen);

                    // Creates a bool in the editor window that is connected to the IsLeftTurningSignalOn on the TurningSignalsScript.
                    // When this bool is selected, it will turn on / off the bool on TurningSignalsScript for IsLeftTurningSignalOn.
                    carSystemsScript.Data.IsLeftTurningSignalOn = EditorGUILayout.Toggle("Left Turning Signal On Toggle", carSystemsScript.Data.IsLeftTurningSignalOn);

                    // Creates a bool in the editor window that is connected to the IsRightTurningSignalOn on the TurningSignalsScript.
                    // When this bool is selected, it will turn on / off the bool on TurningSignalsScript for IsRightTurningSignalOn.
                    carSystemsScript.Data.IsRightTurningSignalOn = EditorGUILayout.Toggle("Right Turning Signal On Toggle", carSystemsScript.Data.IsRightTurningSignalOn);

                    // Creates a bool in the editor window that is connected to the IsHazardFlashingOn on the TurningSignalsScript.
                    // When this bool is selected, it will turn on / off the bool on TurningSignalsScript for IsHazardFlashingOn.
                    carSystemsScript.Data.AreHazardLightsOn = EditorGUILayout.Toggle("Hazard Signal On Toggle", carSystemsScript.Data.AreHazardLightsOn);
                }

                var transmission = VehicleService.GetTransmission();

                // Checking if the Transmission Script is not null
                // If not null, then create variables that are linked to scripts for interaction
                if (transmission != null)
                {
                    // Generating a label in window for PRNDL that is bold
                    GUILayout.Label("PRNDL", EditorStyles.boldLabel);

                    // Generating a dropdown list of items in window for PRNDL
                    var arrayList = new GUIContent("PRNDL");
                    ListIdx = EditorGUILayout.Popup(arrayList, ListIdx, MyList.ToArray());
                }
            }
            else
            {
                // Shows a help box informing user onGui to update data when loading completes
                EditorGUILayout.HelpBox("Once loading completes, mouse over window to update.", MessageType.Info);
               
            }

        }
        else
        {
            EditorGUILayout.HelpBox("Please put editor in play mode to interact with Car services", MessageType.Info);
        }
    }

    /// <summary>
    /// Checking if the loading is completed, and if there is additional loading occuring from other scenes.
    /// </summary>
    /// <returns> returns a true is all loading is completed, and false if there is still loading</returns>
    bool CheckIfSetupIsCompleted()
    {
        if (SceneManager.GetActiveScene().isLoaded)
        {
            if (FindObjectsOfType<SplitScreenSetupConfiguration>().Length > 0)
            {
                SplitScreenScript = FindObjectsOfType<SplitScreenSetupConfiguration>()[0];

            }
            if (SplitScreenScript != null)
            {
                if (SplitScreenScript.AllAsyncLoads.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Unity Update callback
    /// </summary>
    private void Update()
    {
      
        // Checking for a change in PRNDL selection based on list index.
        if (ListIdx != PrevListIdx)
        {
            var transmission = VehicleService.GetTransmission();
            // Updating the Transmission script with the new list index of the PRNDL selection.
            transmission.SwitchToDesiredGear(ListIdx);
            // setting our previous list index for checking in future update checks.
            PrevListIdx = ListIdx;
        }
    }
}
