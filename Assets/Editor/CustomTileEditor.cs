#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CustomTileEditor
{
    [MenuItem("Assets/Create/2D/Custom Tiles/Custom Tile")]
    public static void CreateVariableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save CustomTile", "New Custom Tile", "Asset", "Save Custom Tile", "Assets");
        if (string.IsNullOrEmpty(path)) return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomTile>(), path);
    }
}
#endif