using UnityEngine;
using UnityEngine.Playables;

namespace HMI.UI.Cluster
{
    /// <summary>
    /// Controller for the center part of the cluster
    /// </summary>
    public class CenterController : MonoBehaviour
    {
        /// <summary>
        /// Timeline to move the center of the cluster from Adas to Map Mode
        /// </summary>
        public PlayableAsset AdasToMap;

        /// <summary>
        /// Timeline to move the center of the cluster from Map to Adas Mode
        /// </summary>
        public PlayableAsset MapToAdas;

        /// <summary>
        /// Director that plays timelines
        /// </summary>
        public PlayableDirector Director;

        /// <summary>
        /// Is the center of the cluster currently showing the Adas?
        /// </summary>
        private bool Adas = true;

        /// <summary>
        /// Next screen
        /// </summary>
        public void Next()
        {
            Switch();
        }

        /// <summary>
        /// Previous screen
        /// </summary>
        public void Previous()
        {
            Switch();
        }

        /// <summary>
        /// Switch Screens
        /// </summary>
        private void Switch()
        {
            if (Adas)
            {
                Director.Play(AdasToMap);
            }
            else
            {
                Director.Play(MapToAdas);
            }

            Adas = !Adas;
        }
    }
}
