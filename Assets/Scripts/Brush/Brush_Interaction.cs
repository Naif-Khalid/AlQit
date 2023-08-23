using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush_Interaction : MonoBehaviour
{
 // Reference to texture to draw on
    public Texture2D texture;

    // Default size of the texture
    public Vector2 textureSize = new Vector2(2048, 2048);

    void Start()
    {
         // Get reference to renderer component
     var r = GetComponent<Renderer>();
     
        // Create new texture with specified size
    texture = new Texture2D((int)textureSize.x, (int)textureSize.y);

        // Set texture as main texture for renderer
    r.material.mainTexture = texture;
        
    }
    public void Clear()
    {
        Color[] clearColors = new Color[texture.width * texture.height];
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = Color.white; // Change color 
        }

        texture.SetPixels(clearColors);
        texture.Apply();
    }
}
