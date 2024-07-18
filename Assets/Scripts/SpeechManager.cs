using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    public DrawingManager drawingManager; // Reference to your DrawingManager script

    void Update()
    {
        // Simulate speech recognition using key input for testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnSpeechResult("red");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            OnSpeechResult("blue");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            OnSpeechResult("green");
        }
    }

    void OnSpeechResult(string result)
    {
        Debug.Log("Speech Result: " + result);

        // Change brush color based on simulated speech
        switch (result.ToLower())
        {
            case "red":
                drawingManager.ChangeBrushColor(Color.red);
                break;
            case "blue":
                drawingManager.ChangeBrushColor(Color.blue);
                break;
            case "green":
                drawingManager.ChangeBrushColor(Color.green);
                break;
            case "yellow":
                drawingManager.ChangeBrushColor(Color.yellow);
                break;
            case "light blue":
                drawingManager.ChangeBrushColor(Color.cyan);
                break;
            case "black":
                drawingManager.ChangeBrushColor(Color.black);
                break;
            
            case "pink":
                drawingManager.ChangeBrushColor(Color.magenta);
                break;
            case "gray":
                drawingManager.ChangeBrushColor(Color.gray);
                break;
            // Add more colors as needed
            default:
                Debug.Log("Color not recognized");
                break;
        }
    }
}
