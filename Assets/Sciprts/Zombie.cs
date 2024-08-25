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
    private float moveTick;

    [SerializeField] private MovableObject movableObject;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

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

    private void Update()
    {
        MoveHandler();
        HitHandler();
    }

    #region 이동 함수
    /// <summary>
    /// 좀비의 이동을 제어
    /// </summary>
    private void MoveHandler()
    {
        if (targetPlayer == null)
        {
            moveTick += Time.deltaTime;
            if (moveTick > 1)
            {
                WithoutPlayerMove();
                moveTick = 0;
            }

            Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(5, 5), 0, playerLayer);
            if (player != null)
            {
                targetPlayer = player.transform;
                moveTick = 0;
            }
        }
        else
        {
            // A*
            TargetingMoveWithAStar();
        }
    }

    private void TargetingMoveWithAStar()
    {
        // 가상의 그리드 대신 타일맵을 기준으로 한 플레이어와 좀비의 위치 가져오기
        Vector3Int myTilePos = GameManager.Instance.WallTilemap.WorldToCell(transform.position);
        Vector3Int playerTilePos = GameManager.Instance.WallTilemap.WorldToCell(targetPlayer.position);

        // 시작노드와 플레이어(도착)노드 생성
        Node startNode = new Node(true, myTilePos, 0, 0);
        Node playerNode = new Node(true, playerTilePos, 0, 0);

        // 열린 리스트와 닫힌 리스트 생성
        List<Node> openSet = new();
        List<Node> closeSet = new();

        // 열린 리스트에서 모두 검사하고 FCost가 가장 낮은 노드 찾기
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                // FCost가 더 낮거나 or 같을 때에는 HCost가 더 낮다면 true
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    Debug.Log("currentNode OK");
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode.GridPosition == playerNode.GridPosition)
            {
                RetracePath(startNode, playerNode);
                return;
            }

            foreach (Node neighbor in GetNeighbors(currentNode, GameManager.Instance.WallTilemap))
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
                        openSet.Add(neighbor);
                }
            }
        }
    }

    List<Node> GetNeighbors(Node node, Tilemap tilemap)
    {
        List<Node> neighbors = new List<Node>();

        // 이웃 타일의 상대적인 위치를 정의합니다.
        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = node.GridPosition + direction;
            if (tilemap.HasTile(neighborPos))  // 해당 위치에 타일이 존재하는지 확인합니다.
            {
                bool walkable = true;
                if (tilemap.GetTile(neighborPos) is CustomTile customTile && customTile.TileType != TileTypeID.Ground)
                    walkable = false;

                neighbors.Add(new Node(walkable, neighborPos, node.GCost, node.HCost));
            }
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
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        for(int i =  0; i < path.Count; i++) {
            Debug.Log(path[i].GridPosition);
        }

        // path를 통해 좀비를 움직이게 하거나 그리드를 시각화할 수 있습니다.
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        // X와 Y 좌표 차이의 절대값을 계산합니다.
        int dstX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
        int dstY = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);

        // 맨해튼 거리를 계산합니다.
        return 10 * (dstX + dstY);
    }

    private void WithoutPlayerMove()
    {
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

        StartCoroutine(movableObject.Move(direction, animator, hashMoveX, hashMoveY, hashMove));
    }
    #endregion

    #region 피격 함수
    /// <summary>
    /// 좀비의 피격을 제어
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