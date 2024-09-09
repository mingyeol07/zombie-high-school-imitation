// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Tilemap wallTilemap;
    public Tilemap WallTilemap { get { return wallTilemap; } }

    private Player localPlayer;
    public Player Player => localPlayer;

    private void Awake()
    {
        Instance = this;

        localPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
