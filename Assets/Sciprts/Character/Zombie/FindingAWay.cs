// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public abstract class FindingAWay : MonoBehaviour
{
    public abstract List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition);
}
