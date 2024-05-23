using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public RectTransform drawingCanvas;

    public void SetCanvasSize(float width, float height)
    {
        drawingCanvas.sizeDelta = new Vector2(width, height);
    }

    public void SetCanvasOrientation(string orientation)
    {
        if (orientation == "portrait")
        {
            SetCanvasSize(600, 800); // Example sizes
        }
        else if (orientation == "landscape")
        {
            SetCanvasSize(800, 600);
        }
    
    }
}
