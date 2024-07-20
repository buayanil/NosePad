using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public void SaveTexture(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), fileName);
        File.WriteAllBytes(path, bytes);
        Debug.Log("Drawing saved to: " + path);
    }
}

