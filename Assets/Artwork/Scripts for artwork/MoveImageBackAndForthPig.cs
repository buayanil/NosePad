using UnityEngine;
using System.Collections;

public class MoveImageBackAndForthPig : MonoBehaviour
{
    public RectTransform secondImage; // The UI element to rotate
    public float speed = 15f; // Speed of the rotation
    public float maxAngle = 25f; // Maximum angle to rotate to

    private float currentAngle = 0.0f;
    private bool rotatingBack = false;
    private bool isPaused = true; // Start with pause state

    void Start()
    {
        if (secondImage == null)
        {
            Debug.LogError("Second image RectTransform is not assigned!");
            return;
        }

        StartCoroutine(PauseRoutineWithInitialDelay());
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
        secondImage.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    IEnumerator PauseRoutineWithInitialDelay()
    {
        yield return new WaitForSeconds(5f); // Initial delay

        while (true)
        {
            isPaused = false;
            yield return new WaitForSeconds(5f);
            isPaused = true;
            yield return new WaitForSeconds(5f);
        }
    }
}