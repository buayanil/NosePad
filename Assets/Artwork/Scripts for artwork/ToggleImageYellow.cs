using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleImageYellow : MonoBehaviour
{
    public Image yellow; // The UI Image component

    void Start()
    {
        if (yellow == null)
        {
            Debug.LogError("Image component is not assigned!");
            return;
        }

        // Make sure the image is visible at the start
        yellow.enabled = true;
        StartCoroutine(VisibilityCycle());
    }

    IEnumerator VisibilityCycle()
    {
        // Initial delay of 1 second before starting the cycle
        yield return new WaitForSeconds(5f);

        while (true)
        {
            // Show the image
            yellow.enabled = true;
            yield return new WaitForSeconds(5f);

            // Hide the image
            yellow.enabled = false;
            yield return new WaitForSeconds(5f);
        }
    }
}