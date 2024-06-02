using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class FaceDetector : MonoBehaviour
{
    WebCamTexture _webCamTexture;
    CascadeClassifier faceCascade;
    CascadeClassifier smileCascade;
    OpenCvSharp.Rect myFace;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        _webCamTexture = new WebCamTexture(devices[0].name);
        _webCamTexture.Play();

        faceCascade = new CascadeClassifier(Application.dataPath + @"/haarcascade_frontalface_default.xml");
        smileCascade = new CascadeClassifier(Application.dataPath + @"/haarcascade_smile.xml");
    }

    void Update()
    {
        GetComponent<Renderer>().material.mainTexture = _webCamTexture;
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);
        DetectFaceAndSmile(frame);
        Display(frame);
    }

    void DetectFaceAndSmile(Mat frame)
    {
        // Detect faces
        var faces = faceCascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);

        // If a face is detected, store its position
        if (faces.Length >= 1)
        {
            myFace = faces[0];

            // Define ROI for smile detection
            var faceROI = new Mat(frame, myFace);

            // Detect smiles within the face ROI
            var smiles = smileCascade.DetectMultiScale(faceROI, 1.8, 20);

            // Check if a smile is detected and it is within a reasonable size range
            foreach (var smile in smiles)
            {
                // Adjust these thresholds as needed
                if (smile.Width > myFace.Width / 2 && smile.Height > myFace.Height / 3)
                {
                    // Display "Smile" text if smiling
                    Cv2.PutText(frame, "Smile", new Point(myFace.Left, myFace.Top - 10), HersheyFonts.HersheySimplex, 1.0, Scalar.Red, 2);
                    break; // Exit the loop after the first valid smile detection
                }
            }
        }
    }

    void Display(Mat frame)
    {
        // Draw rectangle around the face if detected
        if (myFace != null)
        {
            frame.Rectangle(myFace, new Scalar(250, 0, 0), 2);
        }

        // Convert Mat to Texture and display on the material
        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }
}
