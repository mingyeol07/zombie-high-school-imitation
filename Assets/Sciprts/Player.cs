using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private float moveSpeed = 0f;
    private Animator animator;

    private readonly int hashMoveX = Animator.StringToHash("PlayerX");
    private readonly int hashMoveY = Animator.StringToHash("PlayerY");
    private readonly int hashMove = Animator.StringToHash("IsMove");

    private float moveX;
    private float moveY;

    private bool isMoving = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorController();

        if (Input.GetKey(KeyCode.RightArrow) && !isMoving)
        {
            MoveRight();
            
        }
        else if(Input.GetKey(KeyCode.LeftArrow) && !isMoving)
        {
            MoveLeft();
            
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !isMoving)
        {
            MoveUp();
           
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isMoving)
        {
            MoveDown();
            
        }
    }

    private void MoveRight()
    {
        StartCoroutine(Co_PlayerMove(Vector2Int.right));
    }

    private void MoveLeft()
    {
        StartCoroutine(Co_PlayerMove(Vector2Int.left));
    }

    private void MoveUp()
    {
        StartCoroutine(Co_PlayerMove(Vector2Int.up));
    }

    private void MoveDown()
    {
        StartCoroutine(Co_PlayerMove(Vector2Int.down));
    }

    private IEnumerator Co_PlayerMove(Vector2Int Direction)
    {
        if(Direction.x == 0)
        {
            moveY = Direction.y;
            moveX = 0;
        }
        else if(Direction.y == 0) 
        {
            moveX = Direction.x;
            moveY = 0;
        }
        animator.SetFloat(hashMoveX, moveX);
        animator.SetFloat(hashMoveY, moveY);

        isMoving = true;

        // 현재 위치의 셀 좌표를 가져옴
        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position);

        // Vector의 방향값을 받아서 지금포지션에 더해줌
        Vector3Int targetCell = myCellPosition + new Vector3Int(Direction.x, Direction.y, 0); 

        // 이동할 셀의 중심 위치를 가져옴
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell); 

        // 목표 지점까지의 거리
        float distance = Vector3.Distance(transform.position, targetPos); 

        // 목표 지점까지 도달하는 데 걸리는 시간 = 목표 지점 간 거리 / 이동 속도
        float duration = distance / moveSpeed;  
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // 정확한 위치로 이동

        isMoving = false;
    }

    private void AnimatorController()
    {
        animator.SetBool(hashMove, isMoving);
    }
}
