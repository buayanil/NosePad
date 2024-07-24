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
    private Vector2? previousNosePosition = null;
    private Vector2? previousCursorPosition = null;
    private float movementThreshold = 20.0f; // Minimum distance to move the cursor

    void Start()
    {
        InitializeTexture();
        undoRedoManager = FindObjectOfType<UndoRedoManager>();
        if (undoRedoManager != null)
        {
            undoRedoManager.SaveStateOnStop(texture);
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
                if (previousNosePosition.HasValue)
                {
                    // Check if the movement exceeds the threshold
                    if (Vector2.Distance(new Vector2(x, y), previousNosePosition.Value) > movementThreshold)
                    {
                        DrawLine((int)previousNosePosition.Value.x, (int)previousNosePosition.Value.y, x, y);
                        previousNosePosition = new Vector2(x, y);
                    }
                }
                else
                {
                    DrawBrush(x, y);
                    previousNosePosition = new Vector2(x, y);
                }
            }
            else
            {
                previousNosePosition = null;
            }
        }
        else
        {
            previousNosePosition = null;
        }
    }

    void DrawBrush(int x, int y)
    {
        Color color = isErasing ? eraseColor : drawColor;
        int brushRadius = brushSize / 2;

        for (int i = -brushRadius; i <= brushRadius; i++)
        {
            for (int j = -brushRadius; j <= brushRadius; j++)
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

    void DrawLine(int x0, int y0, int x1, int y1)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;
        int e2;

        Color color = isErasing ? eraseColor : drawColor;

        while (true)
        {
            DrawBrush(x0, y0);

            if (x0 == x1 && y0 == y1) break;

            e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
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
        Vector2 screenPos = new Vector2(Screen.width - nosePosition.x, Screen.height - nosePosition.y); // Invert Y for correct screen coordinates

        if (!previousCursorPosition.HasValue || Vector2.Distance(screenPos, previousCursorPosition.Value) > movementThreshold)
        {
            cursorImage.rectTransform.position = screenPos;
            previousCursorPosition = screenPos;
        }
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

    public void save()
    {
        saveManager.SaveTexture(texture, "mydrawing.png");
    }

    public void setToggle()
    {
        s = true;
        previousNosePosition = null; // Reset the previous position to start a new line
    }

    public void setToggletofalse()
    {
        if (undoRedoManager != null)
        {
            undoRedoManager.SaveStateOnStop(texture);
        }
        s = false;
        previousNosePosition = null; // Reset the previous position when stopping drawing
    }
}
