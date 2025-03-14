// # Systems
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// # Unity
using UnityEngine;

public class BFS : FindingAWay
{
    protected override void Awake()
    {
        base.Awake();
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    public override List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition)
    {
        Vector3Int startTilePos = groundTileMap.WorldToCell(startPosition);
        Vector3Int endTilePos = groundTileMap.WorldToCell(endPosition);

        Node startNode = GameManager.Instance.GetOrCreateNode(startTilePos);
        Node endNode = GameManager.Instance.GetOrCreateNode(endTilePos);

        Queue<Node> queue = new();
        HashSet<Vector3Int> visited = new();
        Dictionary<Vector3Int, Node> parentMap = new();

        queue.Enqueue(startNode);
        visited.Add(startNode.GridPosition);
        parentMap[startNode.GridPosition] = null;

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

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
                queue.Enqueue(neighbor);
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
