using System;
using UnityEngine;
using UnityEngine.UI;

public class DrawingManager : MonoBehaviour
{
    public RawImage drawingCanvas;
    public int brushSize = 5;
    public Color drawColor = Color.black;
    public Color eraseColor = Color.white;
    private Texture2D texture;
    private bool isErasing = false;
    private UndoRedoManager undoRedoManager;
    public SaveManager saveManager;
    public NoseAndSmileDetector noseAndSmileDetector; // Reference to the NoseAndSmileDetector
    public Image cursorImage; // Reference to the cursor image
    public Boolean s = false;

    void Start()
    {
        InitializeTexture();
        undoRedoManager = FindObjectOfType<UndoRedoManager>();
        if (undoRedoManager != null)
        {
            undoRedoManager.SaveInitialState(texture);
        }

        // Ensure cursor image is visible at the start
        if (cursorImage != null)
        {
            cursorImage.enabled = true;
        }
    }

    void InitializeTexture()
    {
        RectTransform rt = drawingCanvas.rectTransform;
        texture = new Texture2D((int)rt.rect.width, (int)rt.rect.height, TextureFormat.RGBA32, false);
        Color[] colors = new Color[texture.width * texture.height];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        texture.SetPixels(colors);
        texture.Apply();

        drawingCanvas.texture = texture;
    }

    void Update()
    {
       if (s == true)
        {

            DrawWithNose();

        }


        UpdateCursorPosition();
    }




    void DrawWithNose()
    {
        if (noseAndSmileDetector == null)
        {
            Debug.LogError("NoseAndSmileDetector is not assigned");
            return;
        }

        Vector2 localPoint;
        Vector2 nosePosition = noseAndSmileDetector.GetNosePosition();
        Vector2 screenPos = new Vector2(Screen.width - nosePosition.x, Screen.height - nosePosition.y); // Invert X and Y for correct screen coordinates

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, screenPos, null, out localPoint))
        {
            int x = (int)(localPoint.x + drawingCanvas.rectTransform.rect.width / 2);
            int y = (int)(localPoint.y + drawingCanvas.rectTransform.rect.height / 2);

            if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
            {
                DrawBrush(x, y);
            }
        }
    }

    void DrawBrush(int x, int y)
    {
        Color color = isErasing ? eraseColor : drawColor;
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                int drawX = x + i;
                int drawY = y + j;

                if (drawX >= 0 && drawX < texture.width && drawY >= 0 && drawY < texture.height)
                {
                    texture.SetPixel(drawX, drawY, color);
                }
            }
        }
        texture.Apply();
    }

    void UpdateCursorPosition()
    {
        if (noseAndSmileDetector == null || cursorImage == null)
        {
            return;
        }

        Vector2 nosePosition = noseAndSmileDetector.GetNosePosition();
        Vector2 screenPos = new Vector2(nosePosition.x, Screen.height - nosePosition.y); // Invert Y for correct screen coordinates

        cursorImage.rectTransform.position = screenPos;
    }

    public void ToggleEraser(bool erasing)
    {
        isErasing = erasing;
    }

    public void ChangeBrushColor(Color color)
    {
        drawColor = color;
        isErasing = false; // Ensure we're not in erasing mode
    }

    public void ChangeBrushSize(int size)
    {
        brushSize = size;
    }

    public Texture2D GetTexture()
    {
        return texture;
    }

    public void SetCanvasOrientation(bool isPortrait)
    {
        RectTransform rt = drawingCanvas.rectTransform;
        if (isPortrait)
        {
            rt.sizeDelta = new Vector2(600, 800); // Portrait size
        }
        else
        {
            rt.sizeDelta = new Vector2(1700, 850); // Landscape size
        }
        InitializeTexture(); // Reinitialize texture with new size
    }

    public void save(){
        saveManager.SaveTexture(texture, "mydrawing.png");
    }


    public void setToggle()
    {
        s = true;
    }
    public void setToggletofalse()
    {
        s = false;
    }
}
