using UnityEngine;

// This namespace contains the utility classes for creating a demo screen in Unity.
namespace MediaDisplay
{
    // This class provides utility methods for setting up a demo screen to display test images, videos, or webcam streams.
    public static class MediaDisplayManager
    {
        /// <summary>
        /// Sets the texture for the demo screen object.
        /// </summary>
        /// <param name="demoScreenObject">The GameObject to be used as the demo screen.</param>
        /// <param name="displayTexture">The Texture to display on the demo screen.</param>
        public static void SetDemoScreenTexture(GameObject demoScreenObject, Texture displayTexture)
        {
            // Get the MeshRenderer component from the screen object.
            MeshRenderer meshRenderer = demoScreenObject.GetComponent<MeshRenderer>();
            if (meshRenderer == null) return;

            // Set the texture and shader for the screen object's material.
            Material screenMaterial = new Material(Shader.Find("Unlit/Texture"));
            screenMaterial.mainTexture = displayTexture;
            meshRenderer.material = screenMaterial;
        }

        /// <summary>
        /// Updates the rotation, scale, and position of the demo screen object based on the new texture dimensions.
        /// </summary>
        /// <param name="demoScreenObject">The GameObject to be used as the demo screen.</param>
        /// <param name="displayTexture">The Texture displayed on the demo screen.</param>
        /// <param name="mirrorScreen">Optional parameter to mirror the screen horizontally. Default is false.</param>
        public static void UpdateDemoScreenTransform(GameObject demoScreenObject, Texture displayTexture, bool mirrorScreen = false)
        {
            // Set the rotation of the screen object based on the mirrorScreen parameter.
            demoScreenObject.transform.rotation = Quaternion.Euler(0, mirrorScreen ? 180f : 0f, 0);

            // Set the scale of the screen object based on the displayTexture dimensions.
            demoScreenObject.transform.localScale = new Vector3(displayTexture.width, displayTexture.height, mirrorScreen ? -1f : 1f);

            // Set the position of the screen object.
            demoScreenObject.transform.position = new Vector3(displayTexture.width / 2, displayTexture.height / 2, 1);
        }

        /// <summary>
        /// Initializes the camera used for displaying the demo screen.
        /// </summary>
        /// <param name="demoCameraObject">The GameObject with a Camera component to be used for displaying the demo screen.</param>
        /// <param name="screenDimensions">The dimensions of the demo screen.</param>
        public static void InitializeCamera(GameObject demoCameraObject, Vector2Int screenDimensions)
        {
            // Get the Camera component from the camera object.
            Camera camera = demoCameraObject.GetComponent<Camera>();
            if (camera == null) return;

            // Set the position of the camera object.
            demoCameraObject.transform.position = new Vector3(screenDimensions.x / 2, screenDimensions.y / 2, -10f);

            // Set the camera to orthographic mode and set its size.
            camera.orthographic = true;
            camera.orthographicSize = screenDimensions.y / 2;
        }

        /// <summary>
        /// Initializes a webcam stream with the specified settings.
        /// </summary>
        /// <param name="webcamTexture">The WebCamTexture instance to be initialized and played.</param>
        /// <param name="deviceName">The name of the webcam device to be used for streaming.</param>
        /// <param name="webcamDimensions">The desired resolution of the webcam stream.</param>
        /// <param name="webcamFrameRate">The desired frame rate of the webcam stream.</param>
        /// <returns>Returns true if the webcam stream has started playing, false otherwise.</returns>
        public static bool InitializeWebcam(ref WebCamTexture webcamTexture, string deviceName, Vector2Int webcamDimensions, int webcamFrameRate=60)
        {
            // Stop the webcam stream if it's already playing.
            if (webcamTexture != null && webcamTexture.isPlaying)
            {
                webcamTexture.Stop();
            }

            // Create a new WebCamTexture instance with the specified settings.
            webcamTexture = new WebCamTexture(deviceName, webcamDimensions.x, webcamDimensions.y, webcamFrameRate);

            // Start playing the webcam stream.
            webcamTexture.Play();

            // Return true if the webcam stream has started playing, false otherwise.
            return webcamTexture.isPlaying;
        }

        /// <summary>
        /// Updates the texture, transform, and camera of the demo screen object.
        /// </summary>
        /// <param name="demoScreenObject">The GameObject to be used as the demo screen.</param>
        /// <param name="displayTexture">The Texture displayed on the demo screen.</param>
        /// <param name="cameraObject">The GameObject with a Camera component to be used for displaying the demo screen.</param>
        /// <param name="mirrorScreen">Optional parameter to mirror the screen horizontally. Default is false.</param>
        public static void UpdateScreenTexture(GameObject demoScreenObject, Texture displayTexture, GameObject cameraObject, bool mirrorScreen = false)
        {
            // Update the texture of the demo screen object.
            SetDemoScreenTexture(demoScreenObject, displayTexture);

            // Update the transform of the demo screen object based on the new texture dimensions.
            UpdateDemoScreenTransform(demoScreenObject, displayTexture, mirrorScreen);

            // Get the screen dimensions from the updated demo screen object.
            int screenWidth = (int)demoScreenObject.transform.localScale.x;
            int screenHeight = (int)demoScreenObject.transform.localScale.y;
            Vector2Int screenDimensions = new Vector2Int(screenWidth, screenHeight);

            // Initialize the camera for displaying the demo screen.
            InitializeCamera(cameraObject, screenDimensions);
        }
    }
}
