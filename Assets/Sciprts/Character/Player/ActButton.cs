using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격버튼
public class ActButton : MonoBehaviour
{
    public void PointerDown()
    {
        Debug.Log("누름");
    }

    public void PointerUp()
    {
        Debug.Log("뗌");
        GameObject.FindWithTag("Player").GetComponent<Player>().Attack();
    }
}
