using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Transform isTargetPlayer;
    [SerializeField] private LayerMask playerLayer;
    private bool isTargeting;
    private float moveTick;

    [SerializeField] private MovableObject movableObject;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float hitMaxColorTime = 0.5f;
    private float hitCurColorTime;

    private readonly int hashMoveX = Animator.StringToHash("PlayerX");
    private readonly int hashMoveY = Animator.StringToHash("PlayerY");
    private readonly int hashMove = Animator.StringToHash("IsMove");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        moveTick += Time.deltaTime;
        if (!isTargeting && moveTick > 1)
        {
            WithoutPlayerMove();
            moveTick = 0;
        }

        if(isTargetPlayer != null)
        {
            Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(5, 5), 0, playerLayer);
            if(player != null)
            {
                isTargetPlayer = player.transform;
            }
        }
        else
        {
            // A*
            TargetingMove();
        }

        // 색상 변화 처리
        if (hitCurColorTime < hitMaxColorTime)
        {
            hitCurColorTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.red, Color.green, hitCurColorTime / hitMaxColorTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hit();
        }
    }

    private void TargetingMove()
    {

    }

    private void WithoutPlayerMove()
    {
        int RandomDirection = Random.Range(0, 4);
        Vector2Int direction = Vector2Int.zero;
        switch (RandomDirection)
        {
            case 0:
                direction = Vector2Int.down;
                break;
            case 1:
                direction = Vector2Int.up;
                break;
            case 2:
                direction = Vector2Int.right;
                break;
            case 3:
                direction = Vector2Int.left;
                break;
        }

        StartCoroutine(movableObject.Move(direction, animator, hashMoveX, hashMoveY, hashMove));
    }

    [ContextMenu("Hit")]
    public void Hit()
    {
        spriteRenderer.color = Color.red;
        hitCurColorTime = 0f;
    }
}