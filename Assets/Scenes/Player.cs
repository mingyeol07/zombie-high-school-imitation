using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private float moveSpeed = 0f;

    private bool isMoving = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) && !isMoving)
        {
            StartCoroutine(Co_PlayerMove(Vector2Int.right));
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving)
        {
            StartCoroutine(Co_PlayerMove(Vector2Int.left));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !isMoving)
        {
            StartCoroutine(Co_PlayerMove(Vector2Int.up));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isMoving)
        {
            StartCoroutine(Co_PlayerMove(Vector2Int.down));
        }
    }

    private IEnumerator Co_PlayerMove(Vector2Int Direction)
    {
        isMoving = true;

        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position); // 현재 위치를 타일맵 상의 셀 위치로 변환

        Vector3Int targetCell = myCellPosition + new Vector3Int(Direction.x, Direction.y, 0); // 이동할 셀 위치 계산

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
}
