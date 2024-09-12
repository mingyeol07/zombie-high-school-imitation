using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MovableCharacter
{
    private Tilemap tilemap;
    private Animator animator;
    private Transform myTransform;

    private bool isMoving = false;
    public bool IsMoving => isMoving;  
    
    private Vector3Int myTilePosition;
    public Vector3Int MyTilePosition => myTilePosition;

    private int hashMoveX;
    private int hashMoveY;
    private int hashMove;

    public IEnumerator Move(Vector2 direction, float moveSpeed, float moveDelay = 0, Action onMoveComplete = null)
    {
        if (isMoving) yield break;

        animator.SetBool(hashMove, true);
        animator.SetFloat(hashMoveX, direction.x);
        animator.SetFloat(hashMoveY, direction.y);

        Vector3Int myCellPosition = tilemap.WorldToCell(myTransform.position);
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

        float distance = Vector2.Distance(myTransform.position, targetPos);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration && Vector2.Distance(myTransform.position, targetPos) > 0.01f) // targetPos에 거의 도달했는지 확인
        {
            myTransform.position = Vector2.MoveTowards(myTransform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 오브젝트 위치를 정확하게 targetPos로 설정
        myTransform.position = targetPos;

        isMoving = false;
        animator.SetBool(hashMove, false);

        myTilePosition = tilePos;

        if(moveDelay != 0)
        {
            yield return new WaitForSeconds(moveDelay);
        }
        // 코루틴 완료 후 콜백 호출
        onMoveComplete?.Invoke();
    }

    public MovableCharacter(Tilemap wallTilemap, Animator animator,Transform transform,  int hashMoveX, int hashMoveY, int hashMove)
    {
        this.tilemap = wallTilemap;
        this.animator = animator;
        this.hashMoveX = hashMoveX;
        this.hashMoveY = hashMoveY;
        this.hashMove = hashMove;
        this.myTransform = transform;
    }
}
