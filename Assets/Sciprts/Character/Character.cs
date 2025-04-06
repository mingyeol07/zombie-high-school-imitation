// # Systems
using System;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// 캐릭터 상위 클래스
// 이동하지 않는 캐릭터 (ex. 오염시설) 같은 걸 구현하기 위해선 moveableCharacter와 immovableCharacter로 나눌 필요
public class Character : MonoBehaviour
{
    // 이동하면서 체크할 타일 맵
    private Tilemap tilemap;

    private Animator animator;

    protected bool isMoving = false;

    private Vector3Int myTilePosition;
    public Vector3Int MyTilePosition => myTilePosition;

    protected readonly int hashMoveX = Animator.StringToHash("MoveX");
    protected readonly int hashMoveY = Animator.StringToHash("MoveY");
    protected readonly int hashMove = Animator.StringToHash("IsMove");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        tilemap = GameManager.Instance.WallTilemap;
    }

    protected virtual void Update()
    {
        AnimatorController();
    }

    private void AnimatorController()
    {
        animator.SetBool(hashMove, isMoving);
    }

    protected IEnumerator Move(Vector2 direction, float moveSpeed, float moveDelay = 0, Action onMoveComplete = null)
    {
        if (isMoving) yield break;

        animator.SetBool(hashMove, true);
        animator.SetFloat(hashMoveX, direction.x);
        animator.SetFloat(hashMoveY, direction.y);

        // 내 위치에 있는 타일 위치값
        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position);

        // 다음 타일 위치 값
        Vector3Int targetCell = myCellPosition + new Vector3Int((int)direction.x, (int)direction.y, 0);

        // 타일의 월드 위치
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);

        // 타일 위치를 Int로 구해 타일 맵에서 검사할 값으로 변환
        Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(targetPos.x), Mathf.FloorToInt(targetPos.y), Mathf.FloorToInt(targetPos.z));

        if ((tilemap.GetTile(tilePos) is CustomTile customTile && customTile.TileType == TileTypeID.Wall)
            || !GameManager.Instance.GroundTilemap.HasTile(tilePos))
        {
            isMoving = false;
            animator.SetBool(hashMove, false);
            yield break;
        }

        isMoving = true;

        float distance = Vector2.Distance(transform.position, targetPos);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration && Vector2.Distance(transform.position, targetPos) > 0.01f) // targetPos에 거의 도달했는지 확인
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 오브젝트 위치를 정확하게 targetPos로 설정
        transform.position = targetPos;

        isMoving = false;
        animator.SetBool(hashMove, false);

        myTilePosition = tilePos;

        if (moveDelay != 0)
        {
            yield return new WaitForSeconds(moveDelay);
        }
        // 이동 완료 후 콜백 호출
        onMoveComplete?.Invoke();
    }
}
