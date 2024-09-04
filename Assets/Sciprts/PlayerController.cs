using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private bool isTouched;

    private int buttonLayer = LayerMask.GetMask("Button");
}
