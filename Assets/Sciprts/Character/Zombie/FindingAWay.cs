// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class FindingAWay : MonoBehaviour
{
    [SerializeField] protected LineRenderer lineRenderer ;

    Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

    protected virtual void Awake()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public abstract List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition);

    public List<Node> GetNeighbors(Node node, Tilemap tilemap)
    {
        List<Node> neighbors = new List<Node>();
        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = node.GridPosition + direction;
            bool walkable = !tilemap.HasTile(neighborPos);

            //if (tilemap.GetTile(neighborPos) is CustomTile customTile && customTile.TileType != TileTypeID.Ground)
            //{
            //    walkable = false;
            //}
            //else
            //{
            //    // 타일이 없는 경우 (null 타일일 가능성이 있음)
            //    walkable = true;
            //}

            neighbors.Add(new Node(walkable, neighborPos, node.GCost, node.HCost));
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
}
