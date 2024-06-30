using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleImageBlue : MonoBehaviour
{
    public Image blue; // The UI Image component

    void Start()
    {
        if (blue == null)
        {
            Debug.LogError("Image component is not assigned!");
            return;
        }

        // Make sure the image is visible at the start
        blue.enabled = true;
        StartCoroutine(VisibilityCycle());
    }

    IEnumerator VisibilityCycle()
    {
        // Initial delay of 1 second before starting the cycle
        yield return new WaitForSeconds(4f);

        while (true)
        {
            // Show the image
            blue.enabled = true;
            yield return new WaitForSeconds(5f);

            // Hide the image
            blue.enabled = false;
            yield return new WaitForSeconds(5f);
        }
    }
}