// # Systems
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// # Unity
using UnityEngine;

public class Dijkstra : FindingAWay
{
    protected override void Awake()
    {
        base.Awake();
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
    }

    public override List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition)
    {
        Vector3Int startTilePos = GameManager.Instance.WallTilemap.WorldToCell(startPosition);
        Vector3Int endTilePos = GameManager.Instance.WallTilemap.WorldToCell(endPosition);

        Node startNode = GameManager.Instance.GetOrCreateNode(startTilePos);
        Node endNode = GameManager.Instance.GetOrCreateNode(endTilePos);

        List<Node> allNodes = new();
        HashSet<Node> visitedNodes = new();
        Dictionary<Node, Node> parentMap = new();
        PriorityQueue<Node, int> pq = new();

        // 직사각형 범위의 모든 노드 생성
        //Vector3Int minPos = new Vector3Int(Math.Min(startTilePos.x, endTilePos.x), Math.Min(startTilePos.y, endTilePos.y), 0);
        //Vector3Int maxPos = new Vector3Int(Math.Max(startTilePos.x, endTilePos.x), Math.Max(startTilePos.y, endTilePos.y), 0);
        Vector3Int minPos = new Vector3Int(-15, -9, 0);
        Vector3Int maxPos = new Vector3Int(15, 8, 0);

        for (int x = minPos.x; x <= maxPos.x; x++)
        {
            for (int y = minPos.y; y <= maxPos.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                Node node = GameManager.Instance.GetOrCreateNode(tilePos);
                allNodes.Add(node);
            }
        }

        // 다익스트라 알고리즘 초기 설정
        Dictionary<Node, int> distances = new();
        foreach (Node node in allNodes)
        {
            distances[node] = int.MaxValue;
        }
        distances[startNode] = 0;

        pq.Enqueue(startNode, 0);
        parentMap[startNode] = null;

        while (pq.Count > 0)
        {
            Node currentNode = pq.Dequeue();

            if (visitedNodes.Contains(currentNode)) continue;
            visitedNodes.Add(currentNode);

            if (currentNode == endNode)
            {
                DrawSearchPath(visitedNodes.ToList()); // 탐색한 노드들 시각화
                return RetracePath(parentMap, startNode, endNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.Walkable || visitedNodes.Contains(neighbor) || !distances.ContainsKey(neighbor)) continue;

                int newDistance = distances[currentNode] + 1; // 가중치 1로 가정

                if (newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    pq.Enqueue(neighbor, newDistance);
                    parentMap[neighbor] = currentNode;
                }
            }
        }

        DrawSearchPath(visitedNodes.ToList()); // 탐색한 노드들 시각화
        return null;
    }

    private List<Node> RetracePath(Dictionary<Node, Node> parentMap, Node startNode, Node endNode)
    {
        List<Node> path = new();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = parentMap.ContainsKey(currentNode) ? parentMap[currentNode] : null;
        }

        path.Reverse();
        return path;
    }
}

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement element, TPriority priority)> heap = new();

    private int Parent(int index) => (index - 1) / 2;
    private int LeftChild(int index) => 2 * index + 1;
    private int RightChild(int index) => 2 * index + 2;

    public int Count => heap.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        heap.Add((element, priority));
        HeapifyUp(heap.Count - 1);
    }

    public TElement Dequeue()
    {
        if (heap.Count == 0) throw new InvalidOperationException("Queue is empty!");

        TElement minElement = heap[0].element;
        heap[0] = heap[^1]; // 마지막 요소를 루트로 이동
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);

        return minElement;
    }

    public bool IsEmpty() => heap.Count == 0;

    private void HeapifyUp(int index)
    {
        while (index > 0 && heap[index].priority.CompareTo(heap[Parent(index)].priority) < 0)
        {
            (heap[index], heap[Parent(index)]) = (heap[Parent(index)], heap[index]);
            index = Parent(index);
        }
    }

    private void HeapifyDown(int index)
    {
        int smallest = index;
        int left = LeftChild(index);
        int right = RightChild(index);

        if (left < heap.Count && heap[left].priority.CompareTo(heap[smallest].priority) < 0)
            smallest = left;

        if (right < heap.Count && heap[right].priority.CompareTo(heap[smallest].priority) < 0)
            smallest = right;

        if (smallest != index)
        {
            (heap[index], heap[smallest]) = (heap[smallest], heap[index]);
            HeapifyDown(smallest);
        }
    }
}
