using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private float moveSpeed = 0f;
    private Animator animator;

    private bool isMoving = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow) && !isMoving)
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

    private IEnumerator Co_PlayerMove(Vector2Int Direction)
    {
        isMoving = true;

        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position); // 현재 위치의 셀 좌표를 가져옴

        Vector3Int targetCell = myCellPosition + new Vector3Int(Direction.x, Direction.y, 0); // 이동할 셀의 위치

        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell); // 이동할 셀의 중심 위치를 가져옴

        float distance = Vector3.Distance(transform.position, targetPos); // 목표 지점까지의 거리

        float duration = distance / moveSpeed;  // 목표 지점까지 도달하는 데 걸리는 시간 = 목표 지점 간 거리 / 이동 속도

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
}
