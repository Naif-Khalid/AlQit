using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Brush_InteractionMarker : MonoBehaviour
{
// Store references to key components
[SerializeField] private Transform tip;
[SerializeField] private int penSize = 5;
[SerializeField] private TMPro.TMP_InputField penSizeInputField;
// Renderer for drawing on texture
private Renderer renderer;
// Color array for pen tip
private Color[] colors;

// Cache tip height for raycast
private float tipHeight;

// Variables for handling touch input
private RaycastHit touch;
private Brush_Interaction brush_Interaction;
private Vector2 touchPos, lastTouchPos;
private bool wasTouchingLastFrame;
//private Quaternion lastTouchRot;
private int currentPenSize;
// Add a reference to the color picker
public ColorPicker colorPicker;


 // Initialize variables and color array
private void Start()
{
        penSizeInputField.onValueChanged.AddListener(SetPenSize);
            currentPenSize = penSize;
             colorPicker.OnColorChange += UpdateTipColor;

    renderer = tip.GetComponent<Renderer>();
    colors = new Color[penSize * penSize];
    // Fill color array with pen color
    for (int i = 0; i < colors.Length; i++)
    {
        colors[i] = colorPicker.currentColor;
    }

    tipHeight = tip.localScale.y;
}
// Check for touch input and draw
private void Update()
{
     // Update pen color
    for (int i = 0; i < colors.Length; i++)
    {
        colors[i] = colorPicker.currentColor;
    }
    Draw();
}

// Update the color of the tip material
private void UpdateTipColor(Color newColor)
{
    renderer.material.color = newColor;
}

    // Unsubscribe from the color change event
    private void OnDestroy()
    {
        colorPicker.OnColorChange -= UpdateTipColor;
    }

     public void ClearDrawing()
    {
        if (brush_Interaction != null)
        {
            brush_Interaction.Clear();
        }
    }

public void SetPenSize(string size)
{
int newSize = int.Parse(size);
penSize = newSize;
currentPenSize = newSize;
colors = new Color[currentPenSize * currentPenSize];
for (int i = 0; i < colors.Length; i++)
{
colors[i] = renderer.material.color;
}
}
// Raycast for Brush_Interaction and draw ink trail
private void Draw()
{
// Raycast from pen tip
bool isTouching = Physics.Raycast(tip.position, transform.up, out touch, tipHeight);
// Check if hit Brush_Interaction
if (isTouching && touch.transform.CompareTag("Brush_Interaction"))
{
// Cache Brush_Interaction reference
if (brush_Interaction == null)
{
brush_Interaction = touch.transform.GetComponent<Brush_Interaction>();
}
// Get touch position
        touchPos = touch.textureCoord;
         // Calculate start pixel
        int startX = (int)(touchPos.x * brush_Interaction.textureSize.x - (currentPenSize / 2));
        int startY = (int)(touchPos.y * brush_Interaction.textureSize.y - (currentPenSize / 2));

        // Bounds check
        if (startY < 0 || startY > brush_Interaction.textureSize.y || startX < 0 || startX > brush_Interaction.textureSize.x)
        {
            return;
        }

        // If pen was already touching, draw ink trail
        if (wasTouchingLastFrame)
        {
              // Draw dot (x, y , blockWidth, blockhight, color)
            brush_Interaction.texture.SetPixels(startX, startY, penSize, penSize, colors);

              // Interpolate to create smooth trail
            for (float f = 0.01f; f < 1.00f; f += 0.01f)
            {
                int lerpX = (int)Mathf.Lerp(lastTouchPos.x, startX, f);
                int lerpY = (int)Mathf.Lerp(lastTouchPos.y, startY, f);
                brush_Interaction.texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
            }
            // Lerp rotation
          //  transform.rotation = lastTouchRot;
            // Apply to texture
            brush_Interaction.texture.Apply();
        }
        // Cache values
        lastTouchPos = new Vector2(startX, startY);
        //lastTouchRot = transform.rotation;
        wasTouchingLastFrame = true;
    }
    else
    {
        // Reset if not touching
        brush_Interaction = null;
        wasTouchingLastFrame = false;
    }
}
}