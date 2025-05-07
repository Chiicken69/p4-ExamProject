using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Movement : MonoBehaviour
{

    [Header("Movement Settings")]

    [SerializeField] private float movementSpeed;

    [SerializeField] private float dashSpeed;
    // [SerializeField] private bool AllowRBchanges;
     [SerializeField] private Animator animator;





    private Vector3 _moveDir;
    private Rigidbody2D rb;
    //  private Rigidbody2D BackupRB;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        GetMovementInfo();      
        Animate();
    }

    private void FixedUpdate()
    {
        //ChangeRigidBodySettings(AllowRBchanges);
        MovePlayer();
    }


    private void GetMovementInfo()
    {
        _moveDir = InputHandler.Instance.PassInputMoveDir();
    }

    private void MovePlayer()
    {
        rb.AddForce(_moveDir.normalized * movementSpeed);

    }
private Vector2 _lastMoveDir = Vector2.down; // default to Down, can be anything

private void Animate()
{
    bool isMoving = _moveDir.sqrMagnitude > 0.01f;
    Debug.Log("IsMoving: " + isMoving);

    if (isMoving)
        _lastMoveDir = _moveDir.normalized;

    animator.SetFloat("MoveX", _lastMoveDir.x);
    animator.SetFloat("MoveY", _lastMoveDir.y);
    animator.SetBool("IsMoving", isMoving);
}

}