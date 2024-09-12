using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Custom Tile")]
public class CustomTile : Tile
{
    [SerializeField] private TileTypeID tileType;
    public TileTypeID TileType { get { return tileType; } }

    [Header("Tile block")]
    public Vector2Int m_size = Vector2Int.one;
    public Sprite[] m_Sprites;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

       tileData.sprite = GetSprite(position);
    }

    public Sprite GetSprite(Vector3Int pos)
    {
        if (m_Sprites.Length != m_size.x * m_size.y) return sprite;

        while (pos.x < m_size.x) { pos.x += m_size.x; }
        while (pos.y < m_size.y) { pos.y += m_size.y; }

        int x = pos.x % m_size.x;
        int y = pos.y % m_size.y;

        int index = x + (((m_size.y - 1) * m_size.x) - y * m_size.x);

        return m_Sprites[index];
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Custom Tile")]
    public static void CreateVariableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save CustomTile", "New Custom Tile", "Asset", "Save Custom Tile", "Assets");
        if (path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomTile>(), path);
    }
}
#endif