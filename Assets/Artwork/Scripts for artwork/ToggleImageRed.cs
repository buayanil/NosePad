using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleImageRed : MonoBehaviour
{
    public Image red; // The UI Image component

    void Start()
    {
        if (red == null)
        {
            Debug.LogError("Image component is not assigned!");
            return;
        }

        // Make sure the image is visible at the start
        red.enabled = true;
        StartCoroutine(VisibilityCycle());
    }

    IEnumerator VisibilityCycle()
    {
        // Initial delay of 1 second before starting the cycle
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // Show the image
            red.enabled = true;
            yield return new WaitForSeconds(5f);

            // Hide the image
            red.enabled = false;
            yield return new WaitForSeconds(5f);
        }
    }
}