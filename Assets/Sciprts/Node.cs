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

    public int FCost
    {
        get { return GCost + HCost; }
    }

    public Node(bool _walkable, Vector3Int _gridPos, int _gCost, int _hCost)
    {
        Walkable = _walkable;
        GridPosition = _gridPos;
        GCost = _gCost;
        HCost = _hCost;
    }
}