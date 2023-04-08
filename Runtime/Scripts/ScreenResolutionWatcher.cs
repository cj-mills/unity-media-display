using UnityEngine;

// Contains utility classes for creating and managing media displays in Unity.
namespace CJM.MediaDisplay
{
    public class ScreenResolutionWatcher : MonoBehaviour
    {
        // Define a delegate and an event for the resolution change
        public delegate void ResolutionChangedDelegate(Resolution newResolution);
        public static event ResolutionChangedDelegate OnResolutionChanged;

        private Resolution _currentResolution;

        private void Start()
        {
            // Initialize the current resolution value
            _currentResolution = Screen.currentResolution;
        }

        private void Update()
        {
            // Check if the resolution has changed
            if (_currentResolution.width != Screen.currentResolution.width || _currentResolution.height != Screen.currentResolution.height)
            {
                // Update the current resolution value
                _currentResolution = Screen.currentResolution;

                // Invoke the event
                OnResolutionChanged?.Invoke(_currentResolution);

                Debug.Log($"new resolution: {_currentResolution}");
            }
        }
    }
}
