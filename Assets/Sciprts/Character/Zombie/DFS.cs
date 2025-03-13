// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class DFS : FindingAWay
{
    protected override void Awake()
    {
        base.Awake();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    public override List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition)
    {
        Vector3Int startTilePos = GameManager.Instance.WallTilemap.WorldToCell(startPosition);
        Vector3Int endTilePos = GameManager.Instance.WallTilemap.WorldToCell(endPosition);

        Node startNode = new Node(true, startTilePos, 0, 0);
        Node endNode = new Node(true, endTilePos, 0, 0);

        Stack<Node> stack = new();
        HashSet<Vector3Int> visited = new();
        Dictionary<Vector3Int, Node> parentMap = new();

        stack.Push(startNode);
        visited.Add(startNode.GridPosition);
        parentMap[startNode.GridPosition] = null;

        while (stack.Count > 0)
        {
            Node currentNode = stack.Pop();

            if (currentNode.GridPosition == endNode.GridPosition)
            {
                return RetracePath(parentMap, startNode, currentNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode, GameManager.Instance.WallTilemap))
            {
                if (!neighbor.Walkable || visited.Contains(neighbor.GridPosition))
                {
                    continue;
                }

                visited.Add(neighbor.GridPosition); 
                stack.Push(neighbor);
                parentMap[neighbor.GridPosition] = currentNode;
            }
        }

        return new List<Node>(); // 경로를 찾지 못한 경우 빈 리스트 반환
    }

    private List<Node> RetracePath(Dictionary<Vector3Int, Node> parentMap, Node startNode, Node endNode)
    {
        List<Node> path = new();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = parentMap.ContainsKey(currentNode.GridPosition) ? parentMap[currentNode.GridPosition] : null;
        }

        path.Reverse();
        return path;
    }
}
