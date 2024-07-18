using UnityEngine;

public class BrushController : MonoBehaviour
{
    public Renderer brushRenderer;

    public void ChangeBrushColor(Color newColor)
    {
        brushRenderer.material.color = newColor;
    }
}
