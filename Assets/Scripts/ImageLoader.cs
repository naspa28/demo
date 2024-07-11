using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public Image target; // UI Image component to display the image
    public string targetAsset;
    private string localFilePath;

    void Start()
    {
        localFilePath = Path.Combine(Application.persistentDataPath, targetAsset);
        LoadImage();
    }

    private void LoadImage()
    {
        if (File.Exists(localFilePath))
        {
            byte[] imageData = File.ReadAllBytes(localFilePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            // Apply the texture to the UI Image component
            target.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("Image file not found at: " + localFilePath);
        }
    }
}
