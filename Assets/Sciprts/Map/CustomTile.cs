using UnityEngine;
using UnityEngine.Tilemaps;

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
        if (m_Sprites == null || m_Sprites.Length != m_size.x * m_size.y)
            return sprite;  // sprite가 할당되지 않았을 경우 대비

        while (pos.x < 0) pos.x += m_size.x;
        while (pos.y < 0) pos.y += m_size.y;

        int x = pos.x % m_size.x;
        int y = pos.y % m_size.y;

        int index = x + (((m_size.y - 1) * m_size.x) - y * m_size.x);

        if (index < 0 || index >= m_Sprites.Length)
            return sprite; // 배열 범위 초과 방지

        return m_Sprites[index];
    }
}