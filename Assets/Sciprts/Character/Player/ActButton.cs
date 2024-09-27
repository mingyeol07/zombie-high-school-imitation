using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActButton : MonoBehaviour
{
    public void PointerDown()
    {
        Debug.Log("´©¸§");
    }

    public void PointerUp()
    {
        Debug.Log("¶À");
        GameObject.FindWithTag("Player").GetComponent<Player>().Attack();
    }
}
