using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Custom Tile")]
public class CustomTile : Tile
{
    // Ÿ�� Ÿ��
    [SerializeField] private TileTypeID tileType;
    public TileTypeID TileType { get { return tileType; } }
}