using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Zombie : Character
{
    [SerializeField] public Transform targetPlayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 detectionSize = new Vector2(5,5);
    private float moveSpeed = 4;
    private float moveDelay = 0.1f;
    private float moveTick = 1;

    private bool isTargetingMove;
    private SpriteRenderer spriteRenderer;

    private float hitMaxColorTime = 0.5f;
    private float hitCurColorTime;

    private AStar atar;
    private DFS dfs;
    private BFS bfs;
    private Dijkstra dijkstra;

    public bool isAstar;
    public bool isBfs;
    public bool isDfs;
    public bool isDijkstra;

    protected override void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        atar = GetComponent<AStar>();
        dfs = GetComponent<DFS>();
        bfs = GetComponent<BFS>();
        dijkstra = GetComponent<Dijkstra>();
    }

    protected override void Start()
    {
        base.Start();
        WithoutPlayerMove();
    }

    protected override void Update()
    {
        base.Update();
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
            Collider2D player = Physics2D.OverlapBox(transform.position, detectionSize, 0, playerLayer);
            if (player != null)
            {
                targetPlayer = player.transform;
                isTargetingMove = true;
            }
        }
        else if(isTargetingMove)
        {
            if(isMoving)
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
            StartCoroutine(Move(Vector2.zero, moveSpeed, moveDelay, () => { TargetingMove(); }));
            return;
        }

        if (isMoving) return;
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
        StartCoroutine(Move(direction, moveSpeed, moveDelay, () => { TargetingMove(); }));
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

        StartCoroutine(Move(direction, moveSpeed, moveTick, () => { WithoutPlayerMove(); }));
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