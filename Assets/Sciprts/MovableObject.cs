using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MovableObject : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private float moveSpeed = 0f;
    private bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }
    private Vector3Int myTilePosition;
    public Vector3Int MyTilePosition { get { return myTilePosition; } }

    private void Start()
    {
        tilemap = GameManager.Instance.WallTilemap;
    }

    public IEnumerator Move(Vector2 direction, Animator animator, int hashMoveX, int hashMoveY, int hashMove, Action onMoveComplete = null)
    {
        if (isMoving) yield break;

        animator.SetBool(hashMove, true);
        animator.SetFloat(hashMoveX, direction.x);
        animator.SetFloat(hashMoveY, direction.y);

        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position);
        Vector3Int targetCell = myCellPosition + new Vector3Int((int)direction.x, (int)direction.y, 0);
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);
        Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(targetPos.x), Mathf.FloorToInt(targetPos.y), Mathf.FloorToInt(targetPos.z));

        if (tilemap.GetTile(tilePos) is CustomTile customTile && customTile.TileType == TileTypeID.Wall)
        {
            isMoving = false;
            animator.SetBool(hashMove, false);
            yield break;
        }

        isMoving = true;

        float distance = Vector2.Distance(transform.position, targetPos);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
        animator.SetBool(hashMove, false);

        myTilePosition = tilePos;

        // 코루틴 완료 후 콜백 호출
        onMoveComplete?.Invoke();
    }
}
