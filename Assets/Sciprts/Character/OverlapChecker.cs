using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapChecker
{
    private readonly Vector2[] chairmanBoxLocations = { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1) };

    public static Collider2D[] ChairmanAttack()
    {
        return null;
    }

    /// <summary>
    /// �÷��̾ ���� �ٶ󺸴� �������� �ڽ� ũ�⸦ ����
    /// </summary>
    /// <returns></returns>
    public static Collider2D[] GetAttackRange(Transform transform, Vector2 localDirection)
    {
        //Vector2 colliderOffset = (Vector2)transform.position + new Vector2(localDirection.x * chairmanOffset, localDirection.y * chairmanOffset);
        //Collider2D[] colliders = Physics2D.OverlapBoxAll(colliderOffset, chairmanAttack, localAngle, zombieLayer);
        //foreach (Collider2D collider in colliders)
        //{
        //    collider.transform.GetComponent<Zombie>()?.Hit();
        //}

        return null;
    }
}
