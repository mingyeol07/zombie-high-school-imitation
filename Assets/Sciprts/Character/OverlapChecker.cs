using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OverlapChecker
{
    #region 체어맨
    private readonly Vector2 chairmanOffset = new Vector2(0, 1.5f);
    private readonly Vector2 chairmanAttackRange = new Vector2(3, 2);
    #endregion

    #region 히트맨
    private readonly Vector2 hitmanOffset1 = new Vector2(-1f, 0);
    private readonly Vector2 hitmanAttackRange1 = new Vector2(1, 1);

    private readonly Vector2 hitmanOffset2 = new Vector2(0, -1f);
    private readonly Vector2 hitmanAttackRange2 = new Vector2(3, 1);

    private readonly Vector2 hitmanOffset3 = new Vector2(1f, 0);
    private readonly Vector2 hitmanAttackRange3 = new Vector2(1, 1);
    #endregion

    /// <summary>
    /// 플레이어가 앞을 바라보는 기준으로 박스 크기를 정함
    /// </summary>
    /// <returns></returns>
    private Collider2D[] GetAttackRangeInZombies(Transform transform, Vector2 localDirection, int localAngle, LayerMask zombieLayer, Vector2 offset, Vector2 attackRange)
    {
        Vector2 colliderOffset = (Vector2)transform.position + new Vector2(localDirection.x * offset.x, localDirection.y * offset.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(colliderOffset, attackRange, localAngle, zombieLayer);

        return colliders;
    }

    public Collider2D[] GetChairmanAttackRangeInZombies(Transform transform, Vector2 localDirection, int localAngle, LayerMask zombieLayer)
    {
        Collider2D[] colliders = GetAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer, chairmanOffset, chairmanAttackRange);

        return colliders;
    }

    public Collider2D[] GetHitmanAttackRangeInZombies(Transform transform, Vector2 localDirection, int localAngle, LayerMask zombieLayer)
    {
        Collider2D[] colliders1 = GetAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer, hitmanOffset1, hitmanAttackRange1);
        Collider2D[] colliders2 = GetAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer, hitmanOffset2, hitmanAttackRange2);
        Collider2D[] colliders3 = GetAttackRangeInZombies(transform, localDirection, localAngle, zombieLayer, hitmanOffset3, hitmanAttackRange3);

        Collider2D[] colliders = new Collider2D[colliders1.Length + colliders2.Length + colliders3.Length];
        int index = 0;

        foreach (Collider2D collider in colliders1)
        {
            colliders[index++] = collider;
        }
        foreach (Collider2D collider in colliders2)
        {
            colliders[index++] = collider;
        }
        foreach (Collider2D collider in colliders3)
        {
            colliders[index++] = collider;
        }

        return colliders;
    }
}
