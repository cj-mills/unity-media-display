using UnityEngine;
using CJM.MediaDisplay;
using System.Collections;

// Contains utility classes for creating and managing media displays in Unity.
namespace CJM.MediaDisplay
{
    public abstract class BaseScreenManager : MonoBehaviour
    {
        // Scene components and settings
        [Header("Scene")]
        [Tooltip("Screen object in the scene")]
        [SerializeField] protected GameObject screenObject;
        [Tooltip("Camera object in the scene")]
        [SerializeField] protected GameObject cameraObject;
        [Tooltip("A test texture to display on the screen")]
        [SerializeField] protected Texture testTexture;
        [Tooltip("A framerate cap to reduce lag")]
        [SerializeField] protected int maxFrameRate = 500;

        // Webcam settings
        [Header("Webcam")]
        [Tooltip("Option to use webcam as input")]
        [SerializeField] protected bool useWebcam = false;
        [Tooltip("Requested webcam dimensions")]
        [SerializeField] protected Vector2Int webcamDims = new Vector2Int(1280, 720);
        [Tooltip("Requested webcam framerate")]
        [SerializeField, Range(0, 60)] protected int webcamFrameRate = 60;

        // Toggle key settings
        [Header("Toggle Key")]
        [Tooltip("Key to toggle between image and webcam feed")]
        [SerializeField] protected KeyCode toggleKey = KeyCode.Space;

        protected Texture currentTexture;
        protected WebCamDevice[] webcamDevices;
        protected WebCamTexture webcamTexture;
        protected string currentWebcam;

        // Subscribe to the texture change event
        protected virtual void OnEnable()
        {
            TextureChangeEvent.OnMainTextureChanged += HandleMainTextureChanged;
            ScreenResolutionWatcher.OnResolutionChanged += UpdateCurrentTexture;
        }

        // Initializes the application's target frame rate and configures the webcam devices.
        protected virtual void Initialize()
        {
            // Limit the application's target frame rate to reduce lag.
            Application.targetFrameRate = maxFrameRate;
            // Get the list of available webcam devices.
            webcamDevices = WebCamTexture.devices;
            // If no webcam devices are available, disable the useWebcam option.
            useWebcam = webcamDevices.Length > 0 ? useWebcam : false;
            // Set the current webcam device to the first available device, if any.
            currentWebcam = useWebcam ? webcamDevices[0].name : "";
        }

        // Updates the display with the current texture (either a test texture or the webcam feed).
        protected virtual void UpdateDisplay()
        {
            // Set up the webcam if necessary.
            SetupWebcam();
            // Update the current texture based on the useWebcam option.
            UpdateCurrentTexture();
            // Start a coroutine to asynchronously update the screen texture.
            StartCoroutine(UpdateScreenTextureAsync());
        }

        // Sets up the webcam if the useWebcam option is enabled.
        protected void SetupWebcam()
        {
            // If there are no webcam devices, return immediately.
            if (webcamDevices.Length == 0) return;

            // If the useWebcam option is enabled, initialize the webcam.
            if (useWebcam)
            {
                // Initialize the webcam and check if it started playing.
                bool webcamPlaying = MediaDisplayManager.InitializeWebcam(ref webcamTexture, currentWebcam, webcamDims, webcamFrameRate);
                // If the webcam failed to start playing, disable the useWebcam option.
                useWebcam = webcamPlaying ? useWebcam : false;
            }
            // If the useWebcam option is disabled and the webcam texture is playing, stop the webcam.
            else if (webcamTexture != null && webcamTexture.isPlaying)
            {
                webcamTexture.Stop();
            }
        }

        // Updates the current texture and target frame rate based on the useWebcam option.
        protected void UpdateCurrentTexture()
        {
            // Set the current texture to the webcam texture if useWebcam is enabled, otherwise use the test texture.
            currentTexture = useWebcam ? webcamTexture : testTexture;
            // Set the target frame rate based on whether the webcam is being used or not.
            Application.targetFrameRate = useWebcam ? webcamFrameRate * 4 : 500;
        }

        // Coroutine to update the screen texture asynchronously.
        protected IEnumerator UpdateScreenTextureAsync()
        {
            // Wait until the webcamTexture is ready if useWebcam is enabled.
            while (useWebcam && webcamTexture.isPlaying && webcamTexture.width <= 16)
            {
                yield return null;
            }

            // Update the screen texture with the current texture (image or webcam feed).
            MediaDisplayManager.UpdateScreenTexture(screenObject, currentTexture, cameraObject, useWebcam);
        }

        // Any other shared methods or functionality can be added here

        // Handle the texture change event.
        protected virtual void HandleMainTextureChanged(Material material)
        {
            // Update the current texture.
            currentTexture = material.mainTexture;
            // If the new main texture is different from the webcam texture and the webcam is playing, stop the webcam.
            if (webcamTexture && material.mainTexture != webcamTexture && webcamTexture.isPlaying)
            {
                useWebcam = false;
                webcamTexture.Stop();
            }
        }

        protected virtual void HandleResolutionChanged(Resolution newResolution)
        {
            UpdateScreenTextureAsync();
        }

        // Unsubscribe from the texture change event when the script is disabled.
        protected virtual void OnDisable()
        {
            TextureChangeEvent.OnMainTextureChanged -= HandleMainTextureChanged;
            ScreenResolutionWatcher.OnResolutionChanged -= UpdateCurrentTexture;
        }
    }
}