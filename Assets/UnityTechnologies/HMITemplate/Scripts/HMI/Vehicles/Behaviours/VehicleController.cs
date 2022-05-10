using HMI.Vehicles.Services;
using UnityEngine;

namespace HMI.Vehicles.Behaviours
{
    /// <summary>
    /// Controls the vehicle
    /// </summary>
    public class VehicleController : MonoBehaviour
    {
        /// <summary>
        /// Switch vehicle to next gear
        /// </summary>
        public void NextGear()
        {
            VehicleService.GetTransmission().SwitchToNextGear();
        }

        /// <summary>
        /// Switch vehicle to previous gear
        /// </summary>
        public void PreviousGear()
        {
            VehicleService.GetTransmission().SwitchToPreviousGear();
        }

        /// <summary>
        /// Accelerate vehicle
        /// </summary>
        public void Accelerate()
        {
            VehicleService.GetVehicle().Accelerate(1f);
        }

        /// <summary>
        /// Brake vehicle
        /// </summary>
        public void Brake()
        {
            VehicleService.GetVehicle().Brake(1f);
        }

        /// <summary>
        /// Start/Stop Engine
        /// </summary>
        public static void StartStopEngine()
        {
           var engine = VehicleService.GetEngine();

            if(engine.IsEngineOn)
            {
                engine.TurnEngineOff();
            }
            else
            {
                engine.TurnEngineOn();
            }
        }

        /// <summary>
        /// Increase ADAS goal speed
        /// </summary>
        public static void AdasIncreaseGoalSpeed()
        {
            VehicleService.GetSpeedController().IncreaseGoalSpeed();
        }

        /// <summary>
        /// Decrease ADAS goal speed
        /// </summary>
        public static void AdasDecreaseGoalSpeed()
        {
            VehicleService.GetSpeedController().DecreaseGoalSpeed();
        }

        /// <summary>
        /// Set ADAS 
        /// </summary>
        public static void AdasSet()
        {
            VehicleService.GetSpeedController().SetAutomaticSpeedControl();
        }

        /// <summary>
        /// Cancel ADAS command
        /// </summary>
        public static void AdasCancel()
        {
            VehicleService.GetSpeedController().CancelAutomaticSpeedControl();
        }
    }
}
