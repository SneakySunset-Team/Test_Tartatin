using UnityEditor;
using UnityEngine;

public class TTTextureBaker
{
    [MenuItem("Assets/Bake Render Texture to Texture2D")]
    public static void BakeRenderTexture()
    {
        RenderTexture rt = Selection.activeObject as RenderTexture;
        
        if (rt == null)
        {
            EditorUtility.DisplayDialog("Error", "Select a RenderTexture first", "OK");
            return;
        }

        RenderTexture.active = rt;
        Texture2D texture = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Save to disk
        string path = EditorUtility.SaveFilePanelInProject("Save Texture", "BakedTexture", "png", "");
        if (!string.IsNullOrEmpty(path))
        {
            byte[] png = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, png);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Success", $"Saved to {path}", "OK");
        }

        Object.DestroyImmediate(texture);
    }
}
