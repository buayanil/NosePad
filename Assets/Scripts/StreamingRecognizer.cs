using UnityEngine;
using UnityEngine.UI;

public class StreamingRecognizer : MonoBehaviour
{
    public bool enableDebugLogging = false;
    private SpeechColorChange colorChanger;

    private void Awake()
    {
        colorChanger = GetComponent<SpeechColorChange>();
        if (colorChanger == null)
        {
            Debug.LogError("SpeechColorChange component not found.");
            return;
        }
        // Start speech recognition
        StartSpeechRecognition();
    }

    private void StartSpeechRecognition()
    {
        // Simulate speech recognition (replace with actual speech recognition logic)
        // Here, we're just simulating speech recognition by manually setting transcribedText
        string transcribedText = "I want to change the color to red";
        HandleTranscribedSpeech(transcribedText);
    }

    private void HandleTranscribedSpeech(string transcribedText)
    {
        if (enableDebugLogging)
        {
            Debug.Log("Transcribed Text: " + transcribedText);
        }

        // Pass transcribed text to SpeechColorChange component for processing
        colorChanger.HandleTranscribedSpeech(transcribedText);
    }
}
