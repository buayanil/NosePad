using UnityEngine;
using UnityEngine.UI;

public class SpeechColorChange : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas object

    // Method to handle transcribed speech
    public void HandleTranscribedSpeech(string transcribedText)
    {
        // Check for color keywords
        if (transcribedText.ToLower().Contains("red"))
        {
            // Change Canvas color to red
            ChangeCanvasColor(Color.red);
        }
        else if (transcribedText.ToLower().Contains("blue"))
        {
            // Change Canvas color to blue
            ChangeCanvasColor(Color.blue);
        }
        // Add more color keywords and corresponding color changes as needed
    }

    private void ChangeCanvasColor(Color color)
    {
        // Change color of Canvas object
        Graphic canvasGraphic = canvas.GetComponent<Graphic>();
        if (canvasGraphic != null)
        {
            canvasGraphic.color = color;
        }
        else
        {
            Debug.LogError("Graphic component not found on Canvas object.");
        }
    }
}
