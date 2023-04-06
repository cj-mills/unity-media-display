using UnityEngine;

// Contains utility classes for creating and managing media displays in Unity.
namespace CJM.MediaDisplay
{
    public class TextureChangeEvent : MonoBehaviour
    {
        // Define a delegate with the desired signature
        public delegate void OnMainTextureChangedDelegate(Material material);

        // Create a static event with the delegate type
        public static event OnMainTextureChangedDelegate OnMainTextureChanged;

        // Method to call when the mainTexture has been changed
        public static void RaiseMainTextureChangedEvent(Material material)
        {
            OnMainTextureChanged?.Invoke(material);
        }
    }
}
