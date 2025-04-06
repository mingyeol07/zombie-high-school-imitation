using System.Collections;
using UnityEngine;

// 직업군 타입
public enum MyOccupation
{
    Chairman, Hitman,
}

public class Player : Character
{
    [SerializeField] private LayerMask zombieLayer;
    private float moveSpeed = 8;

    private Vector2 localDirection;
    private int localAngle;

    #region 무기 관련
    public MyOccupation occupation;
    [SerializeField] private Animator weaponAnimator;

    private readonly int hashChairmanAttack = Animator.StringToHash("ChairmanAttack");
    private readonly int hashHitmanAttack = Animator.StringToHash("HitmanAttack");
    #endregion

    // 다른 곳에서 플레이어의 위치 검사를 위해 필요한 변수
    // ex. 좀비가 플레이어의 위치와 자신의 위치가 같을 때
    public Vector2 myPos = new Vector2(-0.5f, -0.5f);

    public void MoveByJoystick(MoveDirection dir)
    {
        if (isMoving) return;

        if (dir == MoveDirection.Right)
        {
            localAngle = 90;
            localDirection = Vector2.right;
            StartCoroutine(Move(localDirection, moveSpeed,0, () => { myPos += Vector2.right; }));
        }
        else if (dir == MoveDirection.Left)
        {
            localAngle = -90;
            localDirection = Vector2.left;
            StartCoroutine(Move(localDirection, moveSpeed,0, () => { myPos += Vector2.left; })); 
        }
        else if (dir == MoveDirection.Up)
        {
            localAngle = 180;
            localDirection = Vector2.up;
            StartCoroutine(Move(localDirection, moveSpeed, 0, () => { myPos += Vector2.up; }));
        }
        else if (dir == MoveDirection.Down)
        {
            localAngle = 0;
            localDirection = Vector2.down;
            StartCoroutine(Move(localDirection, moveSpeed, 0, () => { myPos += Vector2.down; }));
        }
    }

    public void Attack()
    {
        weaponAnimator.SetFloat(hashMoveX, localDirection.x);
        weaponAnimator.SetFloat(hashMoveY, localDirection.y);

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
        Collider2D[] colliders = OverlapChecker.GetChairmanAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer);
        foreach (Collider2D collider in colliders)
        {
            collider.transform.GetComponent<Zombie>()?.Hit();
        }
    }
    private void HitmanAttack()
    {
        weaponAnimator.SetTrigger(hashHitmanAttack);
        Collider2D[] colliders = OverlapChecker.GetHitmanAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer);
        foreach (Collider2D collider in colliders)
        {
            collider.transform.GetComponent<Zombie>()?.Hit();
        }
    }
}