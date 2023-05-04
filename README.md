# Unity Media Display

Unity Media Display is a Unity package for creating a demo screen to display test images, videos, or webcam streams. It provides a set of utility functions for setting up a demo screen, updating its texture and transformations, and initializing and managing webcam streams.



## Demo Video
https://user-images.githubusercontent.com/9126128/231288868-2116cc3a-ebbe-4b43-9305-30fea6887bf4.mp4



## Demo Projects

| GitHub Repository                                            | Description                                                  |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| [UnityMediaDisplay_Demo](https://github.com/cj-mills/UnityMediaDisplay_Demo) | A simple demo project demonstrating how to use the Unity-Media-Display and Unity-CV-Image-Gallery packages in Unity. |
| [barracuda-image-classification-demo](https://github.com/cj-mills/barracuda-image-classification-demo) | Perform image classification using computer vision models.   |
| [barracuda-inference-yolox-demo](https://github.com/cj-mills/barracuda-inference-yolox-demo) | Perform object detection using YOLOX models.                 |
| [barracuda-inference-posenet-demo](https://github.com/cj-mills/barracuda-inference-posenet-demo) | Perform 2D human pose estimation using PoseNet models.       |



## Code Walkthrough

* [Code Walkthrough: Unity Media Display Package](https://christianjmills.com/posts/unity-media-display-walkthrough/)



## Features

- Display test images, videos, or webcam streams on a demo screen
- Easily set up and modify demo screen properties like texture, rotation, scale, and position
- Initialize and manage webcam streams with customizable settings



## Getting Started

### Prerequisites

- Unity game engine

### Installation

You can install the unity-media-display package using the Unity Package Manager:

1. Open your Unity project.
2. Go to Window > Package Manager.
3. Click the "+" button in the top left corner, and choose "Add package from git URL..."
4. Enter the GitHub repository URL: `https://github.com/cj-mills/unity-media-display.git`
5. Click "Add". The package will be added to your project.

For Unity versions older than 2021.1, add the Git URL to the `manifest.json` file in your project's `Packages` folder as a dependency:

```json
{
  "dependencies": {
    "com.cj-mills.unity-media-display": "https://github.com/cj-mills/unity-media-display.git",
    // other dependencies...
  }
}
```



## Usage

Here's an example of using the `MediaDisplay.MediaDisplayManager` class:

```c#
using UnityEngine;
using CJM.MediaDisplay;

public class ExampleMediaDisplay : MonoBehaviour
{
    public GameObject screenObject;
    public GameObject cameraObject;
    public Texture testTexture;

    void Start()
    {
        // Set the texture for the demo screen object.
        MediaDisplayManager.SetDemoScreenTexture(screenObject, testTexture);

        // Update the demo screen object's transform based on the texture dimensions.
        MediaDisplayManager.UpdateDemoScreenTransform(screenObject, testTexture);

        // Get the screen dimensions from the demo screen object.
        int screenWidth = (int)screenObject.transform.localScale.x;
        int screenHeight = (int)screenObject.transform.localScale.y;
        Vector2Int screenDimensions = new Vector2Int(screenWidth, screenHeight);

        // Initialize the camera for displaying the demo screen.
        MediaDisplayManager.InitializeCamera(cameraObject, screenDimensions);
    }
}
```



## License

This project is licensed under the MIT License. See the [LICENSE](Documentation~/LICENSE) file for details.
