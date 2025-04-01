using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public enum InputType
    {
        MovementDirection,

    }



    private float InputDirY;
    private float InputDirX;
    private Vector3 MoveDir;

    public static InputHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }



    
    public Vector3 PassInputMoveDir()
    {
        return MoveDir;
    }

    private void ProcessMoveDirection(float vertical, float horizontal)
    {
        MoveDir = new Vector3(horizontal, vertical, 0);
    }
    private void GetInput()
    {
        InputDirY = Input.GetAxisRaw("Horizontal");
        InputDirX = Input.GetAxisRaw("Vertical");

    }




    private void Update()
    {
        GetInput();
        ProcessMoveDirection(InputDirX, InputDirY);

    }
}