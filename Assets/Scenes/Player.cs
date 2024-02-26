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

        Vector3Int myCellPosition = tilemap.WorldToCell(transform.position); // ���� ��ġ�� �� ��ǥ�� ������

        Vector3Int targetCell = myCellPosition + new Vector3Int(Direction.x, Direction.y, 0); // �̵��� ���� ��ġ

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
