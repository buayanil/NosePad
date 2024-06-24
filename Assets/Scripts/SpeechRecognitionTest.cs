using System.Collections;
using System.IO;
using HuggingFace.API;
using TMPro;
using UnityEngine;

public class SpeechRecognitionTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;
    private int sampleWindow = 128;
    private float silenceThreshold = 0.01f; // Adjust this value to set the silence threshold
    private float delayBeforeCheckingSilence = 2.0f; // Delay before starting to check for silence

    private void Start()
    {
        Debug.Log("SpeechRecognitionTest initialized.");
        text.color = Color.white;
        text.text = "Ready to record.";
    }

    private void Update()
    {
        // Check for silence only if recording has started and delay is over
        if (recording && Time.time >= recordingStartTime + delayBeforeCheckingSilence && IsSilent())
        {
            Debug.Log("Silence detected, stopping recording.");
            StopRecording();
        }
    }

    private float recordingStartTime;

    public void StartRecording()
    {
        if (!Microphone.IsRecording(null))
        {
            Debug.Log("Starting recording...");
            text.color = Color.white;
            text.text = "Recording...";
            clip = Microphone.Start(null, false, 10, 44100);
            recording = true;
            recordingStartTime = Time.time;
        }
        else
        {
            Debug.LogWarning("Microphone is already recording.");
        }
    }

    public void StopRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Debug.Log("Stopping recording...");
            var position = Microphone.GetPosition(null);
            Microphone.End(null);
            var samples = new float[position * clip.channels];
            clip.GetData(samples, 0);
            bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
            recording = false;
            SendRecording();
        }
        else
        {
            Debug.LogWarning("Microphone is not recording.");
        }
    }

    private void SendRecording()
    {
        Debug.Log("Sending recording for transcription...");
        text.color = Color.yellow;
        text.text = "Sending...";
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            Debug.Log($"Received transcription: {response}");
            text.color = Color.white;
            text.text = response;
            HandleResponse(response); // Handle the response to change brush color
        }, error => {
            Debug.LogError($"Error during transcription: {error}");
            text.color = Color.red;
            text.text = error;
        });
    }

    private void HandleResponse(string response)
    {
        Debug.Log($"Handling response: {response}");
        if (response.ToLower().Contains("red"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.red);
        }
        else if (response.ToLower().Contains("blue"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.blue);
        }
        else if (response.ToLower().Contains("green"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.green);
        }
        else if (response.ToLower().Contains("pink"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.magenta);
        }
        else if (response.ToLower().Contains("light blue"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.cyan);
        }
        else if (response.ToLower().Contains("black"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.black);
        }
        else if (response.ToLower().Contains("yellow"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.yellow);
        }
        else if (response.ToLower().Contains("gray"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushColor(Color.gray);
        }
        else if (response.ToLower().Contains("undo"))
        {
            FindObjectOfType<UndoRedoManager>().Undo();
        }
        else if (response.ToLower().Contains("small brush"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushSize(2);
        }
        else if (response.ToLower().Contains("big brush"))
        {
            FindObjectOfType<DrawingManager>().ChangeBrushSize(9);
        }

        else if (response.ToLower().Contains("paint"))
        {
            FindObjectOfType<DrawingManager>().setToggle();
        }
        else if (response.ToLower().Contains("stop"))
        {
            FindObjectOfType<DrawingManager>().setToggletofalse();
        }


    }

    private bool IsSilent()
    {
        float[] samples = new float[sampleWindow];
        int startPosition = Microphone.GetPosition(null) - sampleWindow + 1;
        if (startPosition < 0) return false;

        clip.GetData(samples, startPosition);

        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }

        float average = sum / sampleWindow;
        return average < silenceThreshold;
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}
