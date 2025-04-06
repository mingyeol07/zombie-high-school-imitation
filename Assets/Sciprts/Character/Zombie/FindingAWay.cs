// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// 길찾기 상위 클래스
public abstract class FindingAWay : MonoBehaviour
{
    [SerializeField] protected LineRenderer lineRenderer ;
    protected Tilemap groundTileMap; 
    protected Tilemap wallTileMap;

    Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
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

    // 현재 노드를 중심으로 네 방향의 이웃노드들을 타일 맵 기반으로 가져옴
    public List<Node> GetNeighbors(Node currentNode)
    {
        List<Node> neighbors = new List<Node>();
        
        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = currentNode.GridPosition + direction;

            bool isWallTile = wallTileMap.GetTile(neighborPos) is CustomTile customTile && customTile.TileType != TileTypeID.Ground;

            if (!isWallTile && groundTileMap.HasTile(neighborPos))
            {
                Node neighbor = GameManager.Instance.GetOrCreateNode(neighborPos);
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    // 라인 렌더러로 검사한 Node들을  그림
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

    // 라인 렌더러로 검사한 위치들을  그림
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
