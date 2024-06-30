using UnityEngine;

public class MoveImageBackAndForth : MonoBehaviour
{
    public RectTransform image; // The UI element to rotate
    public float speed = 15f; // Speed of the rotation
    public float maxAngle = 25f; // Maximum angle to rotate to

    private float currentAngle = 0.0f;
    private bool rotatingBack = false;

    void Start()
    {
        if (image == null)
        {
            Debug.LogError("Image RectTransform is not assigned!");
            return;
        }
    }

    void Update()
    {
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
}