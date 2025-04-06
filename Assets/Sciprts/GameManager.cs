// # Systems
using System.Collections;
using System.Collections.Generic;
using TMPro;


// # Unity
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// �̵��� ����, �׽�Ʈ ��ư���� ����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // �� Ÿ���� �ִ��� üũ�ϴ� Ÿ�� ��
    [SerializeField] private Tilemap wallTilemap;
    public Tilemap WallTilemap => wallTilemap;

    [SerializeField] private Tilemap groundTilemap;
    public Tilemap GroundTilemap => groundTilemap;

    [SerializeField]  private Player localPlayer;
    public Player Player => localPlayer;

    private List<Zombie> zombieList = new List<Zombie>();

    #region �׽�Ʈ�� ��ư��
    [SerializeField] private Button btn_spawnZombie_1;
    [SerializeField] private Button btn_spawnZombie_10;
    [SerializeField] private Button btn_dfs;
    [SerializeField] private Button btn_bfs;
    [SerializeField] private Button btn_dijkstra;
    [SerializeField] private Button btn_astar;
    #endregion

    [SerializeField] private GameObject zombiePrefab;

    [SerializeField] private TMP_Text txt_zombieCount;

    private Dictionary<Vector3Int, Node> nodeCache = new();

    public Node GetOrCreateNode(Vector3Int position)
    {
        if (!nodeCache.ContainsKey(position))
        {
            nodeCache[position] = new Node(true, position, 0, 0);
        }
        return nodeCache[position];
    }

    private void Awake()
    {
        Instance = this;

        btn_dfs.onClick.AddListener(() =>
        {
            for(int i =0; i < zombieList.Count; i++)
            {
                zombieList[i].ResetPathFinding();
                zombieList[i].isDfs = true;
            }
        });
        btn_astar.onClick.AddListener(() =>
        {
            for (int i = 0; i < zombieList.Count; i++)
            {
                zombieList[i].ResetPathFinding();
                zombieList[i].isAstar = true;
            }
        });
        btn_bfs.onClick.AddListener(() =>
        {
            for (int i = 0; i < zombieList.Count; i++)
            {
                zombieList[i].ResetPathFinding();
                zombieList[i].isBfs = true;
            }
        });
        btn_dijkstra.onClick.AddListener(() =>
        {
            for (int i = 0; i < zombieList.Count; i++)
            {
                zombieList[i].ResetPathFinding();
                zombieList[i].isDijkstra = true;
            }
        });

        btn_spawnZombie_1.onClick.AddListener(() =>
        {
            for (int i = 0; i < 1; i++)
            {
                Zombie z = Instantiate(zombiePrefab).GetComponent<Zombie>();
                SetZombie(z);
            }
        });

        btn_spawnZombie_10.onClick.AddListener(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                Zombie z = Instantiate(zombiePrefab).GetComponent<Zombie>();
                SetZombie(z);
            }
        });
    }

    // ���� ������� ȭ�鿡 �����ֱ�
    public void SetZombie(Zombie zombie)
    {
        zombieList.Add(zombie);
        txt_zombieCount.text = "Zombie Count : " + zombieList.Count;
    }

    public bool IsPlayerTile(Vector2 vec)
    {
        if(vec == Player.myPos) return true;
        return false;
    }
}
