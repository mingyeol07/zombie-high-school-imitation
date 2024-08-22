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

        // ���⿡ ���� �ִϸ����� �Ķ���� ����
        animator.SetFloat(hashMoveX, direction.x);
        animator.SetFloat(hashMoveY, direction.y);
        animator.SetBool(hashMove, true);

        // ���� ��ġ�� �� ��ǥ�� ������
        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position);

        // �̵��� ���� �߽� ��ġ�� ���
        Vector3Int targetCell = myCellPosition + new Vector3Int(direction.x, direction.y, 0);
        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);
        Vector3Int pos = new Vector3Int(Mathf.FloorToInt(targetPos.x), Mathf.FloorToInt(targetPos.y), Mathf.FloorToInt(targetPos.z));

        // Ÿ���� CustomTile�̰�, ������ �˻�
        if (tilemap.GetTile(pos) is CustomTile customTile && customTile.TileType == TileTypeID.Wall)
        {
            isMoving = false;
            animator.SetBool(hashMove, false);
            yield break;
        }

        isMoving = true;

        // �̵� ����
        float distance = Vector3.Distance(transform.position, targetPos);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // ��Ȯ�� ��ġ�� �̵�

        isMoving = false;
        animator.SetBool(hashMove, false);
    }
}