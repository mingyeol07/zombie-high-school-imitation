using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovableObject : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveSpeed = 0f;
    private bool isMoving = false;
    public bool IsMoving {  get { return isMoving; } }

    public IEnumerator Move(Vector2Int direction, Animator animator, int hashMoveX, int hashMoveY, int hashMove)
    {
        if (isMoving) yield break;

        // 방향에 따른 애니메이터 파라미터 설정
        animator.SetFloat(hashMoveX, direction.x);
        animator.SetFloat(hashMoveY, direction.y);
        animator.SetBool(hashMove, true);

        // 현재 위치의 셀 좌표를 가져옴
        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position);

        // 이동할 셀의 중심 위치를 계산
        Vector3Int targetCell = myCellPosition + new Vector3Int(direction.x, direction.y, 0);
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);
        Vector3Int pos = new Vector3Int(Mathf.FloorToInt(targetPos.x), Mathf.FloorToInt(targetPos.y), Mathf.FloorToInt(targetPos.z));

        // 타일이 CustomTile이고, 벽인지 검사
        if (tilemap.GetTile(pos) is CustomTile customTile && customTile.TileType == TileTypeID.Wall)
        {
            isMoving = false;
            animator.SetBool(hashMove, false);
            yield break;
        }

        isMoving = true;

        // 이동 수행
        float distance = Vector3.Distance(transform.position, targetPos);
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
        animator.SetBool(hashMove, false);
    }
}