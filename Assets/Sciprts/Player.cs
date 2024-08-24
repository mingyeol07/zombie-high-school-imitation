using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MovableObject movableObject;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private Animator weaponAnimator;
    private Animator animator;

    private Vector2 localDirection;
    private float localAngle;

    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashAttack = Animator.StringToHash("Attack");

    private readonly float chairmanOffset = 1.5f;
    private readonly Vector2 chairmanAttack = new Vector2(3, 2);

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorController();

        if(!movableObject.IsMoving)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                localAngle = 90;
                localDirection = Vector2.right;
                StartCoroutine(movableObject.Move(localDirection, animator, hashMoveX, hashMoveY, hashMove));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                localAngle = -90;
                localDirection = Vector2.left;
                StartCoroutine(movableObject.Move(localDirection, animator, hashMoveX, hashMoveY, hashMove));
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                localAngle = 180;
                localDirection = Vector2.up;
                StartCoroutine(movableObject.Move(localDirection, animator, hashMoveX, hashMoveY, hashMove));
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                localAngle = 0;
                localDirection = Vector2.down;
                StartCoroutine(movableObject.Move(localDirection, animator, hashMoveX, hashMoveY, hashMove));
            }
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            AttackChairman();
        }
    }

    private void AnimatorController()
    {
        animator.SetBool(hashMove, movableObject != null && movableObject.IsMoving);
    }

    private void AttackChairman()
    {
        weaponAnimator.SetFloat(hashMoveX, localDirection.x);
        weaponAnimator.SetFloat(hashMoveY, localDirection.y);
        weaponAnimator.SetTrigger(hashAttack);

        Vector2 colliderOffset = (Vector2)transform.position + new Vector2(localDirection.x * chairmanOffset, localDirection.y * chairmanOffset);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(colliderOffset, chairmanAttack, localAngle, zombieLayer);
        foreach (Collider2D collider in colliders)
        {
            collider.transform.GetComponent<Zombie>()?.Hit();
        }
    }
}