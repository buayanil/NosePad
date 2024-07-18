using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleImageGreen : MonoBehaviour
{
    public Image green; // The UI Image component

    void Start()
    {
        if (green == null)
        {
            Debug.LogError("Image component is not assigned!");
            return;
        }

        // Make sure the image is visible at the start
        green.enabled = true;
        StartCoroutine(VisibilityCycle());
    }

    IEnumerator VisibilityCycle()
    {
        // Initial delay of 1 second before starting the cycle
        yield return new WaitForSeconds(2f);

        while (true)
        {
            // Show the image
            green.enabled = true;
            yield return new WaitForSeconds(5f);

            // Hide the image
            green.enabled = false;
            yield return new WaitForSeconds(5f);
        }
    }
}