using System.Collections;
using UnityEngine;

public enum MyOccupation
{
    Chairman, Hitman,
}

public class Player : MonoBehaviour
{
    public MyOccupation occupation;

    private MovableCharacter movableObject;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private float moveSpeed;
    private Animator animator;

    private Vector2 localDirection;
    private int localAngle;

    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
    private readonly int hashMove = Animator.StringToHash("IsMove");

    private readonly int hashChairmanAttack = Animator.StringToHash("ChairmanAttack");
    private readonly int hashHitmanAttack = Animator.StringToHash("HitmanAttack");

    private OverlapChecker checker = new OverlapChecker();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        movableObject = new MovableCharacter(GameManager.Instance.WallTilemap, animator, transform, hashMoveX, hashMoveY, hashMove);
    }

    private void Update()
    {
        AnimatorController();

        if (Input.GetKeyUp(KeyCode.J))
        {
            weaponAnimator.SetFloat(hashMoveX, localDirection.x);
            weaponAnimator.SetFloat(hashMoveY, localDirection.y);
            Attack();
        }
    }

    public void MoveToJoy(int MoveVec)
    {
        if (movableObject.IsMoving) return;

        if (MoveVec == 1)
        {
            localAngle = 90;
            localDirection = Vector2.right;
            StartCoroutine(movableObject.Move(localDirection, moveSpeed));
        }
        else if (MoveVec == 2)
        {
            localAngle = -90;
            localDirection = Vector2.left;
            StartCoroutine(movableObject.Move(localDirection, moveSpeed));
        }
        else if (MoveVec == 3)
        {
            localAngle = 180;
            localDirection = Vector2.up;
            StartCoroutine(movableObject.Move(localDirection, moveSpeed));
        }
        else if (MoveVec == 4)
        {
            localAngle = 0;
            localDirection = Vector2.down;
            StartCoroutine(movableObject.Move(localDirection, moveSpeed));
        }
    }

    private void AnimatorController()
    {
        animator.SetBool(hashMove, movableObject != null && movableObject.IsMoving);
    }

    private void Attack()
    {
        switch (occupation)
        {
            case MyOccupation.Hitman:
                HitmanAttack();
                break;
            case MyOccupation.Chairman:
                ChairmanAttack();
                break;
        }
    }

    private void ChairmanAttack()
    {
        weaponAnimator.SetTrigger(hashChairmanAttack);
        Collider2D[] colliders = checker.GetChairmanAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer);
        foreach (Collider2D collider in colliders)
        {
            collider.transform.GetComponent<Zombie>()?.Hit();
        }
    }
    private void HitmanAttack()
    {
        weaponAnimator.SetTrigger(hashHitmanAttack);
        Collider2D[] colliders = checker.GetHitmanAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer);
        foreach (Collider2D collider in colliders)
        {
            collider.transform.GetComponent<Zombie>()?.Hit();
        }
    }
}