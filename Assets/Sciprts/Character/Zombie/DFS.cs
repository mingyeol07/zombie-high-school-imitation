// # Systems
using System.Collections;
using System.Collections.Generic;
using System.Linq;


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
        Vector3Int startTilePos = groundTileMap.WorldToCell(startPosition);
        Vector3Int endTilePos = groundTileMap.WorldToCell(endPosition);

        Node startNode = GameManager.Instance.GetOrCreateNode(startTilePos);
        Node endNode = GameManager.Instance.GetOrCreateNode(endTilePos);

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
                DrawSearchPath(visited.ToArray());
                return RetracePath(parentMap, startNode, currentNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
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

        return null;
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
