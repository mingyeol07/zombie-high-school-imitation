using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 overlapBoxSize = new Vector2(5,5);
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float moveDelay = 0.1f;
    [SerializeField] private float moveTick = 1;
    private bool isTargetingMove;

    private MovableCharacter movableObject;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private List<Node> movePath;

    private float hitMaxColorTime = 0.5f;
    private float hitCurColorTime;

    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
    private readonly int hashMove = Animator.StringToHash("IsMove");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        movableObject = new MovableCharacter(GameManager.Instance.WallTilemap,animator, transform, hashMoveX, hashMoveY, hashMove);
        WithoutPlayerMove();
    }

    private void Update()
    {
        MoveHandler();
        HitHandler();
    }

    #region �̵� �Լ�
    /// <summary>
    /// ������ �̵��� ����
    /// </summary>
    private void MoveHandler()
    {
        if (targetPlayer == null)
        {
            Collider2D player = Physics2D.OverlapBox(transform.position, overlapBoxSize, 0, playerLayer);
            if (player != null)
            {
                targetPlayer = player.transform;
                isTargetingMove = true;
            }
        }
        else if(isTargetingMove)
        {
            if(movableObject.IsMoving)
            {
                return;
            }
            else
            {
                TargetingMove();
                isTargetingMove = false;
            }
        }
    }

    private void TargetingMove()
    {
        TargetingMoveWithAStar();
        MoveAlongPath();
    }

    private void TargetingMoveWithAStar()
    {
        Debug.Log("TargetingMoveWithAStar ȣ���");

        Vector3Int myTilePos = GameManager.Instance.WallTilemap.WorldToCell(transform.position);
        Vector3Int playerTilePos = GameManager.Instance.WallTilemap.WorldToCell(targetPlayer.position);

        Node startNode = new Node(true, myTilePos, 0, 0);
        Node playerNode = new Node(true, playerTilePos, 0, 0);

        List<Node> openSet = new();
        List<Node> closeSet = new();

        openSet.Add(startNode);
        Debug.Log($"���� ���: {startNode.GridPosition}, �÷��̾� ���: {playerNode.GridPosition}");

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

            if (currentNode.GridPosition == playerNode.GridPosition)
            {
                Debug.Log("�÷��̾� ��忡 �����߽��ϴ�.");
                RetracePath(startNode, currentNode); // �� ��带 playerNode���� currentNode�� ����
                return;
            }

            foreach (Node neighbor in GetNeighbors(currentNode, GameManager.Instance.WallTilemap))
            {
                if (!neighbor.Walkable || closeSet.Contains(neighbor))
                {
                    Debug.Log($"�̿� ��� �ǳʶ�: {neighbor.GridPosition}, �̵� ���� ����: {neighbor.Walkable}");
                    continue;
                }

                int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, playerNode);
                    neighbor.Parent = currentNode;  // Parent ����
                    Debug.Log($"�̿� ��� �θ� ����: {neighbor.GridPosition}�� �θ�� {currentNode.GridPosition}�Դϴ�.");

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                        Debug.Log($"�̿� ��� �߰���: {neighbor.GridPosition}");
                    }
                }
            }
        }

        Debug.LogError("�÷��̾�� ���� ��θ� ã�� ���߽��ϴ�.");
    }


    List<Node> GetNeighbors(Node node, Tilemap tilemap)
    {
        List<Node> neighbors = new List<Node>();

        // �̿� Ÿ���� ������� ��ġ�� �����մϴ�.
        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = node.GridPosition + direction;
            bool walkable = true;

            if (tilemap.GetTile(neighborPos) is CustomTile customTile && customTile.TileType != TileTypeID.Ground)
            {
                walkable = false;
            }
            else
            {
                // Ÿ���� ���� ��� (null Ÿ���� ���ɼ��� ����)
                walkable = true;
            }

            if (tilemap.HasTile(neighborPos)) walkable = false;

            neighbors.Add(new Node(walkable, neighborPos, node.GCost, node.HCost));
        }

        return neighbors;
    }

    void RetracePath(Node startNode, Node endNode)
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

        Debug.Log(currentNode.GridPosition);

        for(int i = 0 ; i < path.Count; i++)
        {
            Debug.Log(path[i].GridPosition);
        }

        if (path == null || path.Count == 0)
        {
            Debug.Log("Path is null or empty!");
            return;
        }

        movePath = path;
    }

    private void MoveAlongPath()
    {
        if (movePath == null || movePath.Count == 0)
        {
            // �� �ٲ� �ʿ伺 ����.
            Debug.Log("MovePath is null or empty!");
            StartCoroutine(movableObject.Move(Vector2.zero, moveSpeed, moveDelay, () => { TargetingMove(); }));
            return;
        }

        if (movableObject.IsMoving) return;

        Vector3 nextStep = movePath[0].GridPosition;
        movePath.RemoveAt(0);

        Vector3 direction = (nextStep - GameManager.Instance.WallTilemap.WorldToCell(transform.position)).normalized;
        StartCoroutine(movableObject.Move(direction, moveSpeed, moveDelay, () => { TargetingMove(); }));
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        // X�� Y ��ǥ ������ ���밪�� ����մϴ�.
        int dstX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
        int dstY = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);

        // ����ư �Ÿ��� ����մϴ�.
        return dstX + dstY;
    }

    private void WithoutPlayerMove()
    {
        if(targetPlayer != null) return;
        int RandomDirection = Random.Range(0, 4);
        Vector2Int direction = Vector2Int.zero;

        switch (RandomDirection)
        {
            case 0:
                direction = Vector2Int.down;
                break;
            case 1:
                direction = Vector2Int.up;
                break;
            case 2:
                direction = Vector2Int.right;
                break;
            case 3:
                direction = Vector2Int.left;
                break;
        }

        StartCoroutine(movableObject.Move(direction, moveSpeed, moveTick, () => { WithoutPlayerMove(); }));
    }
    #endregion

    #region �ǰ� �Լ�
    /// <summary>
    /// ������ �ǰ��� ����
    /// </summary>
    private void HitHandler()
    {
        if (hitCurColorTime < hitMaxColorTime)
        {
            hitCurColorTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.red, Color.green, hitCurColorTime / hitMaxColorTime);
        }
    }

    public void Hit()
    {
        spriteRenderer.color = Color.red;
        hitCurColorTime = 0f;
    }
    #endregion
}