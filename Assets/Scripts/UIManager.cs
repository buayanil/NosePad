using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public CanvasController canvasController;
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;

    public void ApplyCustomSize()
    {
        float width = float.Parse(widthInput.text);
        float height = float.Parse(heightInput.text);
        canvasController.SetCanvasSize(width, height);
    }

    public void SetPortraitOrientation()
    {
        canvasController.SetCanvasOrientation("portrait");
    }

    public void SetLandscapeOrientation()
    {
        canvasController.SetCanvasOrientation("landscape");
    }
}
