using UnityEngine;
using System.Collections;

public class MoveImageBackAndForthNose : MonoBehaviour
{
    public RectTransform image; // The UI element to rotate
    public float speed = 20f; // Speed of the rotation
    public float maxAngle = 35f; // Maximum angle to rotate to

    private float currentAngle = 0.0f;
    private bool rotatingBack = false;
    private bool isPaused = false;

    void Start()
    {
        if (image == null)
        {
            Debug.LogError("Image RectTransform is not assigned!");
            return;
        }

        StartCoroutine(PauseRoutine());
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (!rotatingBack)
        {
            // Rotate counterclockwise until reaching maxAngle
            currentAngle += speed * Time.deltaTime;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                rotatingBack = true;
            }
        }
        else
        {
            // Rotate clockwise back to the starting position
            currentAngle -= speed * Time.deltaTime;
            if (currentAngle <= 0.0f)
            {
                currentAngle = 0.0f;
                rotatingBack = false;
            }
        }

        // Apply the rotation
        image.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    IEnumerator PauseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            isPaused = true;
            yield return new WaitForSeconds(5f);
            isPaused = false;
        }
    }
}