using System;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public enum InputType
    {
        MovementDirection,Scroll

    }



    private float InputDirY;
    private float InputDirX;
    private float Scroll;

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

    public float PassInputVloatValue()
    {
        return Scroll;
    }

    private void ProcessMoveDirection(float vertical, float horizontal)
    {
        MoveDir = new Vector3(horizontal, vertical, 0);
    }
 
    private void GetInput()
    {
        InputDirY = Input.GetAxisRaw("Horizontal");
        InputDirX = Input.GetAxisRaw("Vertical");

        Scroll = Input.GetAxis("Mouse ScrollWheel");

    }




    private void Update()
    {
        GetInput();
        ProcessMoveDirection(InputDirX, InputDirY);

    }
}