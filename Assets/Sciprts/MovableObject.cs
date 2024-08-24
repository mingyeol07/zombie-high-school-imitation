using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovableObject : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private float moveSpeed = 0f;
    private bool isMoving = false;
    public bool IsMoving {  get { return isMoving; } }
    private Vector3Int myTilePosition;
    public Vector3Int MyTilePosition { get { return myTilePosition; } }

    private void Start()
    {
        tilemap = GameManager.Instance.WallTilemap;
    }

    public IEnumerator Move(Vector2 direction, Animator animator, int hashMoveX, int hashMoveY, int hashMove)
    {
        if (isMoving) yield break;

        // 방향에 따른 애니메이터 파라미터 설정
        animator.SetBool(hashMove, true);
        animator.SetFloat(hashMoveX, direction.x);
        animator.SetFloat(hashMoveY, direction.y);

        // 현재 위치의 셀 좌표를 가져옴
        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position);

        // 이동할 셀의 중심 위치를 계산
        Vector3Int targetCell = myCellPosition + new Vector3Int((int)direction.x, (int)direction.y, 0);
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);

        // GetTile로 위치의 해당하는 타일을 가져오기 위한 Vector3Int 변수
        Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(targetPos.x), Mathf.FloorToInt(targetPos.y), Mathf.FloorToInt(targetPos.z));

        // 타일이 CustomTile이고, 벽인지 검사
        if (tilemap.GetTile(tilePos) is CustomTile customTile && customTile.TileType == TileTypeID.Wall)
        {
            isMoving = false;
            animator.SetBool(hashMove, false);
            yield break;
        }

        isMoving = true;

        // 이동 수행
        float distance = Vector2.Distance(transform.position, targetPos);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // 정확한 위치로 이동

        isMoving = false;
        animator.SetBool(hashMove, false);

        myTilePosition = tilePos;
    }
}