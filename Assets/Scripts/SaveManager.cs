using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public void SaveTexture(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, bytes);
        Debug.Log("Drawing saved to: " + path);
    }
}
