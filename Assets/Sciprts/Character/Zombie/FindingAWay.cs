// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class FindingAWay : MonoBehaviour
{
    [SerializeField] protected LineRenderer lineRenderer ;
    protected Tilemap groundTileMap;
    protected Tilemap wallTileMap;

    Vector3Int[] directions = {
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, 0, 0),
        };

    protected virtual void Awake()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    private void Start()
    {
        groundTileMap = GameManager.Instance.GroundTilemap;
        wallTileMap = GameManager.Instance.WallTilemap;
    }

    public abstract List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition);

    public void ResetLine()
    {
        lineRenderer.gameObject.SetActive(false);
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        
        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = node.GridPosition + direction;

            bool isWallTile = wallTileMap.GetTile(neighborPos) is CustomTile customTile && customTile.TileType != TileTypeID.Ground;

            if (!isWallTile && groundTileMap.HasTile(neighborPos))
            {
                Node neighbor = GameManager.Instance.GetOrCreateNode(neighborPos);
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        // X와 Y 좌표 차이의 절대값을 계산합니다.
        int dstX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
        int dstY = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);

        // 맨해튼 거리를 계산합니다.
        return dstX + dstY;
    }

    protected void DrawSearchPath(List<Node> searchedNodes)
    {
        if (searchedNodes == null || searchedNodes.Count == 0) return;

        lineRenderer.gameObject.SetActive(true);
        lineRenderer.positionCount = searchedNodes.Count;

        for (int i = 0; i < searchedNodes.Count; i++)
        {
            lineRenderer.SetPosition(i, GameManager.Instance.WallTilemap.CellToWorld(searchedNodes[i].GridPosition) + new Vector3(0.5f, 0.5f, 0));
        }
    }

    protected void DrawSearchPath(Vector3Int[] searchedNodes)
    {
        if (searchedNodes == null || searchedNodes.Length == 0) return;

        lineRenderer.gameObject.SetActive(true);
        lineRenderer.positionCount = searchedNodes.Length;

        for (int i = 0; i < searchedNodes.Length; i++)
        {
            lineRenderer.SetPosition(i, GameManager.Instance.WallTilemap.CellToWorld(searchedNodes[i]) + new Vector3(0.5f, 0.5f, 0));
        }
    }
}
