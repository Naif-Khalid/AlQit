using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ColorPicker : MonoBehaviour
{
    public GameObject colorPickerPanel;
    public Button colorPickerButton;
    public Button[] colorButtons;
    public Color[] colors;
    public Color currentColor;
    public delegate void ColorChangeAction(Color newColor);
    public event ColorChangeAction OnColorChange;


    // Start  before the first frame update
    void Start()
    {

        colorPickerPanel.SetActive(false);

        colorPickerButton.onClick.AddListener(() => colorPickerPanel.SetActive(!colorPickerPanel.activeSelf));
        
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i; // Copy i to avoid closure problem
            colorButtons[i].onClick.AddListener(() => SetColor(index));
        }
    }

    void SetColor(int index)
    {
       currentColor = colors[index];
        
        // Call the OnColorChange event
        OnColorChange?.Invoke(currentColor);
        
        colorPickerPanel.SetActive(false);
    }
}