using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using System.IO;

public class NoseAndSmileDetector : MonoBehaviour
{
    WebCamTexture _webCamTexture;
    CascadeClassifier faceCascade;
    CascadeClassifier noseCascade;
    CascadeClassifier smileCascade;
    OpenCvSharp.Rect myFace;
    OpenCvSharp.Rect myNose;
    Vector2[] initialNosePoints = new Vector2[7];
    List<Vector2[]> nosePointsBuffer = new List<Vector2[]>();
    int bufferSize = 5;
    bool initialNosePointsSet = false;
    float movementThreshold = 50.0f;
    float cooldownTime = 1.0f;
    float lastClickTime;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            _webCamTexture = new WebCamTexture(devices[0].name);
            _webCamTexture.Play();
            Debug.Log("Webcam initialized: " + devices[0].name);
        }
        else
        {
            Debug.LogError("No webcam found");
            return;
        }

        string faceCascadePath = Path.Combine(Application.dataPath, "haarcascade_frontalface_default.xml");
        string noseCascadePath = Path.Combine(Application.dataPath, "haarcascade_mcs_nose.xml");
        string smileCascadePath = Path.Combine(Application.dataPath, "haarcascade_smile.xml");

        faceCascade = new CascadeClassifier(faceCascadePath);
        noseCascade = new CascadeClassifier(noseCascadePath);
        smileCascade = new CascadeClassifier(smileCascadePath);

        if (faceCascade.Empty() || noseCascade.Empty() || smileCascade.Empty())
        {
            Debug.LogError("Failed to load cascade classifiers");
        }

        lastClickTime = Time.time - cooldownTime; // Initialize to allow immediate first click
    }

    void Update()
    {
        if (_webCamTexture == null || faceCascade.Empty() || noseCascade.Empty() || smileCascade.Empty())
        {
            return;
        }

        GetComponent<Renderer>().material.mainTexture = _webCamTexture;
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);
        DetectFaceNoseAndSmile(frame);
        Display(frame);
    }

    void DetectFaceNoseAndSmile(Mat frame)
    {
        var faces = faceCascade.DetectMultiScale(frame, 1.1, 5, HaarDetectionType.ScaleImage, new Size(100, 100));

        if (faces.Length >= 1)
        {
            myFace = faces[0];
            var faceROI = new Mat(frame, myFace);
            var noses = noseCascade.DetectMultiScale(faceROI, 1.3, 5, HaarDetectionType.ScaleImage, new Size(30, 30));
            var smiles = smileCascade.DetectMultiScale(faceROI, 1.1, 10, HaarDetectionType.ScaleImage, new Size(15, 15)); // Adjusted for higher sensitivity

            if (noses.Length >= 1)
            {
                myNose = noses[0];
                myNose.X += myFace.X;
                myNose.Y += myFace.Y;

                Vector2[] currentNosePoints = GetNosePoints();
                nosePointsBuffer.Add(currentNosePoints);

                if (nosePointsBuffer.Count > bufferSize)
                {
                    nosePointsBuffer.RemoveAt(0);
                }

                // Use the nose points for cursor tracking
                Vector2 noseCursorPosition = new Vector2(myNose.X + myNose.Width / 2, myNose.Y + myNose.Height / 2);
                // Implement the cursor movement with the nose here, e.g., drawing logic

                if (!initialNosePointsSet)
                {
                    initialNosePoints = GetAveragedNosePoints();
                    initialNosePointsSet = true;
                }
            }
            else
            {
                myNose = OpenCvSharp.Rect.Empty;
            }

            foreach (var smile in smiles)
            {
                if (smile.Width > myFace.Width / 3 && smile.Height > myFace.Height / 4 && Time.time >= lastClickTime + cooldownTime)
                {
                    Debug.Log("Smile detected! Click entered!");
                    lastClickTime = Time.time;
                    break; // Exit the loop after the first valid smile detection
                }
            }
        }
        else
        {
            myFace = OpenCvSharp.Rect.Empty;
            myNose = OpenCvSharp.Rect.Empty;
        }
    }

    Vector2[] GetNosePoints()
    {
        return new Vector2[]
        {
            new Vector2(myNose.X + myNose.Width / 2, myNose.Y),
            new Vector2(myNose.X + myNose.Width / 2, myNose.Y + myNose.Height / 4),
            new Vector2(myNose.X + myNose.Width / 2, myNose.Y + myNose.Height / 2),
            new Vector2(myNose.X + myNose.Width / 2, myNose.Y + myNose.Height * 3 / 4),
            new Vector2(myNose.X + myNose.Width / 2, myNose.Y + myNose.Height),
            new Vector2(myNose.X + myNose.Width / 10, myNose.Y + myNose.Height * 2 / 3),
            new Vector2(myNose.X + myNose.Width * 9 / 10, myNose.Y + myNose.Height * 2 / 3)
        };
    }

    Vector2[] GetAveragedNosePoints()
    {
        Vector2[] averagedPoints = new Vector2[7];

        foreach (var points in nosePointsBuffer)
        {
            for (int i = 0; i < points.Length; i++)
            {
                averagedPoints[i] += points[i];
            }
        }

        for (int i = 0; i < averagedPoints.Length; i++)
        {
            averagedPoints[i] /= nosePointsBuffer.Count;
        }

        return averagedPoints;
    }

    void Display(Mat frame)
    {
        if (myFace != OpenCvSharp.Rect.Empty)
        {
            frame.Rectangle(myFace, new Scalar(250, 0, 0), 2);
        }

        if (myNose != OpenCvSharp.Rect.Empty)
        {
            Vector2[] nosePoints = GetAveragedNosePoints();
            Scalar dotColor = new Scalar(0, 0, 255);

            foreach (var point in nosePoints)
            {
                frame.Circle(new Point(point.x, point.y), 3, dotColor, -1);
            }
        }

        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }
}
