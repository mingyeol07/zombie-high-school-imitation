using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ݹ�ư
public class ActButton : MonoBehaviour
{
    public void PointerDown()
    {
        Debug.Log("����");
    }

    public void PointerUp()
    {
        Debug.Log("��");
        GameObject.FindWithTag("Player").GetComponent<Player>().Attack();
    }
}
