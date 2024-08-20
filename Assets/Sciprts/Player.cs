using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MovableObject movableObject;
    private Animator animator;

    private readonly int hashMoveX = Animator.StringToHash("PlayerX");
    private readonly int hashMoveY = Animator.StringToHash("PlayerY");
    private readonly int hashMove = Animator.StringToHash("IsMove");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorController();

        if (Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(movableObject.Move(Vector2Int.right, animator, hashMoveX, hashMoveY, hashMove));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine(movableObject.Move(Vector2Int.left, animator, hashMoveX, hashMoveY, hashMove));
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            StartCoroutine(movableObject.Move(Vector2Int.up, animator, hashMoveX, hashMoveY, hashMove));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine(movableObject.Move(Vector2Int.down, animator, hashMoveX, hashMoveY, hashMove));
        }
    }

    private void AnimatorController()
    {
        animator.SetBool(hashMove, movableObject != null && movableObject.IsMoving);
    }
}