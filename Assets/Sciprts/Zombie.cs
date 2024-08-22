using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Transform isTargetPlayer;
    private bool isTargeting;
    private float moveTick;

    [SerializeField] private MovableObject movableObject;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float colorR;
    private float colorG;
    private float colorB;

    private readonly float hitColorTime =1f;

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
        if(!isTargeting && moveTick > 1)
        {
            WithoutPlayerMove();
            moveTick = 0;
        }

        if (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, Time.deltaTime / hitColorTime);
        }

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
    }
}
