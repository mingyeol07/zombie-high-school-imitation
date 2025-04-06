// # Systems
using System;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// ĳ���� ���� Ŭ����
// �̵����� �ʴ� ĳ���� (ex. �����ü�) ���� �� �����ϱ� ���ؼ� moveableCharacter�� immovableCharacter�� ���� �ʿ�
public class Character : MonoBehaviour
{
    // �̵��ϸ鼭 üũ�� Ÿ�� ��
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

        // �� ��ġ�� �ִ� Ÿ�� ��ġ��
        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position);

        // ���� Ÿ�� ��ġ ��
        Vector3Int targetCell = myCellPosition + new Vector3Int((int)direction.x, (int)direction.y, 0);

        // Ÿ���� ���� ��ġ
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);

        // Ÿ�� ��ġ�� Int�� ���� Ÿ�� �ʿ��� �˻��� ������ ��ȯ
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

        while (elapsedTime < duration && Vector2.Distance(transform.position, targetPos) > 0.01f) // targetPos�� ���� �����ߴ��� Ȯ��
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ������Ʈ ��ġ�� ��Ȯ�ϰ� targetPos�� ����
        transform.position = targetPos;

        isMoving = false;
        animator.SetBool(hashMove, false);

        myTilePosition = tilePos;

        if (moveDelay != 0)
        {
            yield return new WaitForSeconds(moveDelay);
        }
        // �̵� �Ϸ� �� �ݹ� ȣ��
        onMoveComplete?.Invoke();
    }
}
