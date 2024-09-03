using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isTouched;
    public bool IsTouched => isTouched;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isTouched = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isTouched = false;
    }
}
