using UnityEngine;
using System.Collections.Generic;

public class UndoRedoManager : MonoBehaviour
{
    private Stack<Texture2D> stateStack = new Stack<Texture2D>();
    private DrawingManager drawingManager;

    void Start()
    {
        drawingManager = FindObjectOfType<DrawingManager>();
    }

    public void SaveStateOnStop(Texture2D texture)
    {
        if (drawingManager != null)
        {
            // Clone the texture and push it onto the stack
            Texture2D textureClone = new Texture2D(texture.width, texture.height, texture.format, false);
            textureClone.SetPixels(texture.GetPixels());
            textureClone.Apply();
            stateStack.Push(textureClone);
            Debug.Log("State Saved on Stop Command");

            // Keep only the last two states
            if (stateStack.Count > 2)
            {
                stateStack = new Stack<Texture2D>(stateStack.ToArray()[..2]);
            }
        }
        else
        {
            Debug.LogError("DrawingManager component is not found.");
        }
    }

    public void Undo()
    {
        if (stateStack.Count > 1)
        {
            stateStack.Pop(); // Remove the current state
            Texture2D lastState = stateStack.Peek(); // Get the previous state

            // Apply the lastState texture to the current texture
            Texture2D currentTexture = drawingManager.GetTexture();
            currentTexture.SetPixels(lastState.GetPixels());
            currentTexture.Apply();
            Debug.Log("Undo Performed");
        }
        else
        {
            Debug.LogWarning("Nothing to undo");
        }
    }
}