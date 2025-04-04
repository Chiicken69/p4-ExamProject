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
        rb.AddForce(_moveDir * movementSpeed);

    }

    /*  private void ChangeRigidBodySettzings(bool ChangeRB)
      {
          if (!ChangeRB)
          {
              rb.

          }

          if (!ChangeRB) return;

          rb.gravityScale = 1;



      } */


}