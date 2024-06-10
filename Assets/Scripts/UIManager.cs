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
    public Button colorButton1;
    public Button colorButton2;
    public Button colorButton3;
    public Button colorButton4;
    public Button colorButton5;
    public Button colorButton6;
    public Button colorButton7;
    public Button colorButton8;
    public TMP_Dropdown brushSizeDropdown;
    public Button toggleOrientationButton;
    private bool isPortrait = false;

    void Start()
    {
        eraserToggle.onValueChanged.AddListener(drawingManager.ToggleEraser);
        saveButton.onClick.AddListener(SaveDrawing);
        undoButton.onClick.AddListener(undoRedoManager.Undo);

        colorButton1.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.red));
        colorButton2.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.green));
        colorButton3.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.blue));
        colorButton4.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.black));
        colorButton5.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.grey));
        colorButton6.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.magenta));
        colorButton7.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.cyan));
        colorButton8.onClick.AddListener(() => drawingManager.ChangeBrushColor(Color.yellow));

        brushSizeDropdown.onValueChanged.AddListener(ChangeBrushSize);
        toggleOrientationButton.onClick.AddListener(ToggleCanvasOrientation);
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

    void ToggleCanvasOrientation()
    {
        isPortrait = !isPortrait;
        drawingManager.SetCanvasOrientation(isPortrait);
        toggleOrientationButton.GetComponentInChildren<TextMeshProUGUI>().text = isPortrait ? "Landscape" : "Portrait";
    }

    void SaveDrawing()
    {
        saveManager.SaveTexture(drawingManager.GetTexture(), "drawing.png");
    }
}
