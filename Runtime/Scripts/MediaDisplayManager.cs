using UnityEngine;

// Contains utility classes for creating and managing media displays in Unity.
namespace CJM.MediaDisplay
{
    // Provides utility methods for setting up and managing a media display screen in Unity.
    public static class MediaDisplayManager
    {
        /// <summary>
        /// Sets the texture for the screen object and raises a texture change event.
        /// </summary>
        /// <param name="screenObject">The GameObject to be used as the screen.</param>
        /// <param name="displayTexture">The Texture to display on the screen.</param>
        public static void SetScreenTexture(GameObject screenObject, Texture displayTexture)
        {
            // Attempt to get the MeshRenderer component from the screen object
            if (screenObject.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
            {
                // Create a new material with the Unlit/Texture shader and set its main texture
                Material screenMaterial = new Material(Shader.Find("Unlit/Texture"))
                {
                    mainTexture = displayTexture
                };

                // Assign the material to the mesh renderer
                meshRenderer.material = screenMaterial;

                // Raise a texture change event to inform other scripts
                TextureChangeEvent.RaiseMainTextureChangedEvent(meshRenderer.material);
            }
        }

        /// <summary>
        /// Updates the screen object's rotation, scale, and position based on the display texture and mirror settings.
        /// </summary>
        /// <param name="screenObject">The GameObject to be used as the screen.</param>
        /// <param name="displayTexture">The Texture displayed on the screen.</param>
        /// <param name="mirrorScreen">Optional parameter to mirror the screen horizontally. Default is false.</param>
        public static void UpdateScreenTransform(GameObject screenObject, Texture displayTexture, bool mirrorScreen = false)
        {
            // Get the width and height of the display texture
            float width = displayTexture.width;
            float height = displayTexture.height;

            // Set the rotation, scale, and position of the screen object
            screenObject.transform.rotation = Quaternion.Euler(0, mirrorScreen ? 180f : 0f, 0);
            screenObject.transform.localScale = new Vector3(width, height, mirrorScreen ? -1f : 1f);
            screenObject.transform.position = new Vector3(width / 2, height / 2, 1);
        }

        /// <summary>
        /// Initializes the camera used for displaying the screen.
        /// </summary>
        /// <param name="cameraObject">The GameObject with a Camera component to be used for displaying the screen.</param>
        /// <param name="screenDimensions">The dimensions of the screen.</param>
        public static void InitializeCamera(GameObject cameraObject, Vector2Int screenDimensions)
        {
            // Attempt to get the Camera component from the camera object
            if (cameraObject.TryGetComponent<Camera>(out Camera camera))
            {
                // Set the position of the camera object
                Vector3 position = new Vector3(screenDimensions.x / 2, screenDimensions.y / 2, -10f);
                cameraObject.transform.position = position;

                // Configure the camera for orthographic mode
                camera.orthographic = true;

                // Calculate the aspect ratios of the screen object and the camera's viewport
                float screenAspectRatio = (float)screenDimensions.x / screenDimensions.y;
                float cameraAspectRatio = (float)camera.pixelWidth / camera.pixelHeight;

                // Set the camera's orthographic size based on the aspect ratios
                if (screenAspectRatio > cameraAspectRatio)
                {
                    camera.orthographicSize = screenDimensions.y / 2 / camera.aspect;
                }
                else
                {
                    camera.orthographicSize = screenDimensions.y / 2;
                }
            }
        }


        /// <summary>
        /// Initializes and plays a webcam stream with the specified settings.
        /// </summary>
        /// <param name="webcamTexture">A reference to the WebCamTexture instance to be initialized and played.</param>
        /// <param name="deviceName">The name of the webcam device to be used for streaming.</param>
        /// <param name="webcamDimensions">The desired resolution of the webcam stream.</param>
        /// <param name="webcamFrameRate">The desired frame rate of the webcam stream. Default is 60.</param>
        /// <returns>Returns true if the webcam stream has started playing, false otherwise.</returns>
        public static bool InitializeWebcam(ref WebCamTexture webcamTexture, string deviceName, Vector2Int webcamDimensions, int webcamFrameRate = 60)
        {
            // If the webcam texture is not null and it is playing, stop it
            if (webcamTexture != null && webcamTexture.isPlaying)
            {
                webcamTexture.Stop();
            }

            // Create a new WebCamTexture instance with the specified settings
            webcamTexture = new WebCamTexture(deviceName, webcamDimensions.x, webcamDimensions.y, webcamFrameRate);

            // Start playing the webcam stream
            webcamTexture.Play();

            // Return true if the webcam stream has started playing, false otherwise
            return webcamTexture.isPlaying;
        }

        /// <summary>
        /// Updates the texture, transform, and camera of the screen object.
        /// </summary>
        /// <param name="screenObject">The GameObject to be used as the screen.</param>
        /// <param name="displayTexture">The Texture displayed on the screen.</param>
        /// <param name="cameraObject">The GameObject with a Camera component to be used for displaying the screen.</param>
        /// <param name="mirrorScreen">Optional parameter to mirror the screen horizontally. Default is false.</param>
        public static void UpdateScreenTexture(GameObject screenObject, Texture displayTexture, GameObject cameraObject, bool mirrorScreen = false)
        {
            // Update the texture of the screen object
            SetScreenTexture(screenObject, displayTexture);

            // Update the transform of the screen object based on the new texture dimensions and mirror settings
            UpdateScreenTransform(screenObject, displayTexture, mirrorScreen);

            // Get the screen dimensions from the updated screen object
            Vector2Int screenDimensions = new Vector2Int(displayTexture.width, displayTexture.height);

            // Initialize the camera for displaying the screen
            InitializeCamera(cameraObject, screenDimensions);
        }

    }
}
