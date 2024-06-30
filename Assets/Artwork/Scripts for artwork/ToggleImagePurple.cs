using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleImagePurple : MonoBehaviour
{
    public Image purple; // The UI Image component

    void Start()
    {
        if (purple == null)
        {
            Debug.LogError("Image component is not assigned!");
            return;
        }

        // Make sure the image is visible at the start
        purple.enabled = true;
        StartCoroutine(VisibilityCycle());
    }

    IEnumerator VisibilityCycle()
    {
        // Initial delay of 1 second before starting the cycle
        yield return new WaitForSeconds(3f);

        while (true)
        {
            // Show the image
            purple.enabled = true;
            yield return new WaitForSeconds(5f);

            // Hide the image
            purple.enabled = false;
            yield return new WaitForSeconds(5f);
        }
    }
}