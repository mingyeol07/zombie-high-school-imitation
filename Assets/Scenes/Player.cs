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

        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position); // ���� ��ġ�� Ÿ�ϸ� ���� �� ��ġ�� ��ȯ

        Vector3Int targetCell = myCellPosition + new Vector3Int(Direction.x, Direction.y, 0); // �̵��� �� ��ġ ���

        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell); // �̵��� ���� �߽� ��ġ�� ������

        float distance = Vector3.Distance(transform.position, targetPos); // ��ǥ ���������� �Ÿ�

        float duration = distance / moveSpeed;  // ��ǥ �������� �����ϴ� �� �ɸ��� �ð� = ��ǥ ���� �� �Ÿ� / �̵� �ӵ�

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // ��Ȯ�� ��ġ�� �̵�

        isMoving = false;
    }
}
