// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class BFS : FindingAWay
{
    protected override void Awake()
    {
        base.Awake();
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    public override List<Node> GetMovePath(Vector2 startPosition, Vector2 endPosition)
    {
        throw new System.NotImplementedException();
    }
}
