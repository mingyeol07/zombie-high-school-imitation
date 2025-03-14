// # Systems
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// # Unity
using UnityEngine;

public class AStar : FindingAWay
{
    protected override void Awake()
    {
        base.Awake();
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    public override List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition)
    {
        Debug.Log("TargetingMoveWithAStar 호출됨");

        Vector3Int myTilePos = GameManager.Instance.WallTilemap.WorldToCell(startPosition);
        Vector3Int playerTilePos = GameManager.Instance.WallTilemap.WorldToCell(endPosition);

        Node startNode = GameManager.Instance.GetOrCreateNode(myTilePos);
        Node playerNode = GameManager.Instance.GetOrCreateNode(playerTilePos);

        List<Node> openSet = new();
        HashSet<Node> closeSet = new();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            // 목표 노드 도착 시 경로 찾기
            if (currentNode.GridPosition == playerNode.GridPosition)
            {
                Debug.Log("플레이어 노드에 도착했습니다.");
                DrawSearchPath(closeSet.ToList());
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.Walkable || closeSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, playerNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        Debug.LogError("플레이어로 가는 경로를 찾지 못했습니다.");
        //DrawSearchPath(closeSet.ToList());  // 탐색 경로 시각화
        return null;
    }

    private new int GetDistance(Node nodeA, Node nodeB)
    {
        // X와 Y 좌표 차이의 절대값을 계산합니다.
        int dstX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
        int dstY = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);

        // 맨해튼 거리를 계산합니다.
        return dstX + dstY;
    }
    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            if (currentNode.Parent == null) break;
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
        //DrawPath(path); // 경로 시각화 추가
    }
}
