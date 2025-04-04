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
    private bool Interact;

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
    public bool PassInputBoolValue()
    {
        return Interact;
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

        Interact = Input.GetKey(KeyCode.E);

    }




    private void Update()
    {
        GetInput();
        ProcessMoveDirection(InputDirX, InputDirY);

    }
}