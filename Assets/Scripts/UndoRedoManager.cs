using UnityEngine;
using System.Collections.Generic;

public class UndoRedoManager : MonoBehaviour
{
    private Stack<Color[]> undoStack = new Stack<Color[]>();
    private Stack<Color[]> redoStack = new Stack<Color[]>();
    private DrawingManager drawingManager;

    void Start()
    {
        drawingManager = FindObjectOfType<DrawingManager>();
    }

    public void SaveInitialState(Texture2D texture)
    {
        undoStack.Push((Color[])texture.GetPixels().Clone());
    }

    public void SaveState(Texture2D texture)
    {
        if (drawingManager != null)
        {
            undoStack.Push((Color[])texture.GetPixels().Clone());
            redoStack.Clear();  // Clear the redo stack
            Debug.Log("State Saved");
        }
        else
        {
            Debug.LogError("DrawingManager component is not found.");
        }
    }

    public void Undo()
    {
        if (undoStack.Count > 1)  // Keep initial state
        {
            redoStack.Push((Color[])drawingManager.GetTexture().GetPixels().Clone());
            drawingManager.GetTexture().SetPixels(undoStack.Pop());
            drawingManager.GetTexture().Apply();
            Debug.Log("Undo Performed");
        }
        else
        {
            Debug.LogWarning("Nothing to undo");
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            undoStack.Push((Color[])drawingManager.GetTexture().GetPixels().Clone());
            drawingManager.GetTexture().SetPixels(redoStack.Pop());
            drawingManager.GetTexture().Apply();
            Debug.Log("Redo Performed");
        }
        else
        {
            Debug.LogWarning("Nothing to redo");
        }
    }
}