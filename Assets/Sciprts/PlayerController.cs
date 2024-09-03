using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private bool isTouched;

    private int buttonLayer = LayerMask.GetMask("Button");

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            TouchPhase touchPhase = touch.phase;

            if (touchPhase == TouchPhase.Began)
            {
                isTouched = true;
            }
            else if (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled)
            {
                isTouched = false;
            }

            if (isTouched)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, 0, buttonLayer))
                {
                    if(hit.transform.gameObject.TryGetComponent<JoyButton>(out JoyButton joy))
                    {
                        
                    }
                }
            }
        }
        else if (Input.touchCount == 2)
        {
            
        }
        else
        {
            
        }
    }
}
