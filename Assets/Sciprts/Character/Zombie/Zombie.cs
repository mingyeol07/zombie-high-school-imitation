using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Zombie : MonoBehaviour
{
    [SerializeField] public Transform targetPlayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 overlapBoxSize = new Vector2(5,5);
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float moveDelay = 0.1f;
    [SerializeField] private float moveTick = 1;
    private bool isTargetingMove;

    private MovableCharacter movableObject;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float hitMaxColorTime = 0.5f;
    private float hitCurColorTime;

    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
    private readonly int hashMove = Animator.StringToHash("IsMove");

    private AStar atar;
    private DFS dfs;
    private BFS bfs;
    private Dijkstra dijkstra;

    public bool isAstar;
    public bool isBfs;
    public bool isDfs;
    public bool isDijkstra;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        atar = GetComponent<AStar>();
        dfs = GetComponent<DFS>();
        bfs = GetComponent<BFS>();
        dijkstra = GetComponent<Dijkstra>();
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

    #region 이동 함수
    /// <summary>
    /// 좀비의 이동을 제어
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

    public void ResetPathFinding()
    {
        isAstar = false;
        isBfs = false;
        isDfs = false;
        isDijkstra = false;

        dijkstra.ResetLine();
        atar.ResetLine();
        dfs.ResetLine();
        bfs.ResetLine();
    }

    private void TargetingMove()
    {
        List<Node> path = null;
        if (isAstar) path = atar.GetMovePath(transform.position, targetPlayer.position);
        else if (isBfs) path = bfs.GetMovePath(transform.position, targetPlayer.position);
        else if (isDfs) path = dfs.GetMovePath(transform.position, targetPlayer.position);
        else if (isDijkstra) path = dijkstra.GetMovePath(transform.position, targetPlayer.position);
        MoveAlongPath(path);
    }

    private void MoveAlongPath(List<Node> movePath)
    {
        if (movePath == null || movePath.Count == 0)
        {
            StartCoroutine(movableObject.Move(Vector2.zero, moveSpeed, moveDelay, () => { TargetingMove(); }));
            return;
        }

        if (movableObject.IsMoving) return;
        Debug.Log(GameManager.Instance.WallTilemap.WorldToCell(transform.position));
        for (int i =0; i < movePath.Count; i++)
        {
            Debug.Log(movePath[i].GridPosition);
        }

        Vector3 nextStep;
        Vector3 myPos;
        do
        {
            nextStep = movePath[0].GridPosition;
            movePath.RemoveAt(0);

            myPos = GameManager.Instance.WallTilemap.WorldToCell(transform.position);
        } while (nextStep == myPos && movePath.Count  > 0);

        Vector3 direction = (nextStep - myPos).normalized;
        StartCoroutine(movableObject.Move(direction, moveSpeed, moveDelay, () => { TargetingMove(); }));
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