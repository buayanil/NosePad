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

    void Start()
    {
        InitializeTexture();
        undoRedoManager = FindObjectOfType<UndoRedoManager>();
        if (undoRedoManager != null)
        {
            undoRedoManager.SaveInitialState(texture);
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
        HandleDrawing();
    }

    void HandleDrawing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (undoRedoManager != null)
            {
                undoRedoManager.SaveState(texture);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, Input.mousePosition, null, out localPoint))
            {
                int x = (int)(localPoint.x + drawingCanvas.rectTransform.rect.width / 2);
                int y = (int)(localPoint.y + drawingCanvas.rectTransform.rect.height / 2);

                if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                {
                    DrawBrush(x, y);
                }
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
}