using UnityEngine;
using TMPro;

public class CommandPopupManager : MonoBehaviour
{
    public Canvas commandPopupCanvas;
    public TextMeshProUGUI commandText;

    private void Start()
    {
        if (commandPopupCanvas != null)
        {
            commandPopupCanvas.gameObject.SetActive(false); // Ensure the canvas is initially disabled
        }
    }

    public void ShowCommands()
    {
        if (commandPopupCanvas != null)
        {
            commandText.text = GetCommandList();
            commandPopupCanvas.gameObject.SetActive(true);
        }
    }

    public void HideCommands()
    {
        if (commandPopupCanvas != null)
        {
            commandPopupCanvas.gameObject.SetActive(false);
        }
    }

    private string GetCommandList()
    {
        return "Available Commands:\n" +
               "- stop: Stop recording and save state\n" +
               "- undo: Undo the last action\n" +
               "- red: Change brush color to red\n" +
               "- blue: Change brush color to blue\n" +
               "- green: Change brush color to green\n" +
               "- pink: Change brush color to pink\n" +
               "- light blue: Change brush color to light blue\n" +
               "- black: Change brush color to black\n" +
               "- yellow: Change brush color to yellow\n" +
               "- gray: Change brush color to gray\n" +
               "- save: Save the drawing\n" +
               "- small brush: Change to a small brush size\n" +
               "- big brush: Change to a big brush size\n" +
               "- paint: Start painting\n" +
               "- stop paint: Stop painting\n" +
               "- big brush red: Change to a big red brush\n";
    }
}
