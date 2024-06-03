using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public DrawingManager drawingManager;
    public SaveManager saveManager;
    public UndoRedoManager undoRedoManager;
    public Toggle eraserToggle;
    public Button saveButton;
    public Button undoButton;
    public Button redoButton;
    public Button colorButton1;
    public Button colorButton2;
    public Button colorButton3;
    public Button colorButton4;
    public TMP_Dropdown brushSizeDropdown;

    void Start()
    {
        eraserToggle.onValueChanged.AddListener(drawingManager.ToggleEraser);
        saveButton.onClick.AddListener(SaveDrawing);
        undoButton.onClick.AddListener(undoRedoManager.Undo);
        redoButton.onClick.AddListener(undoRedoManager.Redo);

        colorButton1.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.red));
        colorButton2.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.green));
        colorButton3.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.blue));
        colorButton4.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.black));

        brushSizeDropdown.onValueChanged.AddListener(ChangeBrushSize);
    }

     void ChangeBrushSize(int index)
    {
        // Map dropdown index to brush size
        int size = 5; // Default size
        switch (index)
        {
            case 0:
                size = 5; // Small
                break;
            case 1:
                size = 15; // Medium
                break;
            case 2:
                size = 30; // Large
                break;
        }
        drawingManager.ChangeBrushSize(size);
    }

    void SaveDrawing()
    {
        saveManager.SaveTexture(drawingManager.GetTexture(), "drawing.png");
    }
}
