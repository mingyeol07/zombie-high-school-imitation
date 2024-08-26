// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3Int GridPosition;
    public int GCost;
    public int HCost;
    public Node Parent;

    public Node(bool walkable, Vector3Int gridPosition, int gCost, int hCost)
    {
        Walkable = walkable;
        GridPosition = gridPosition;
        GCost = gCost;
        HCost = hCost;
        Parent = null;
    }

    public int FCost => GCost + HCost;
}