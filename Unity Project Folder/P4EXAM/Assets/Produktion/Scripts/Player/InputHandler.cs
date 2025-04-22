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
    private bool CloseUI;
    private bool DownArrow;
    private bool UpArrow;
    private bool LeftArrow;
    private bool RightArrow;
    private bool openBlueprintUI;
    private bool LeftMouseButton;
    private bool RightMouseButtonDown;
    private bool RightMouseButtonUp;
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

    public Vector3 PassMousePosInWorld()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    public float PassInputFloatValue()
    {
        return Scroll;
    }

    

    public bool PassInputBoolValue(int key)
    {
        switch(key)
        {
            case 1:
            return Interact;
            case 2:
            return CloseUI;
            case 3:
                return RightArrow;
            case 4:
                return LeftArrow;
            case 5:
                return DownArrow;
            case 6:
                return UpArrow;
            case 7:
                return openBlueprintUI;
            case 8:
                return LeftMouseButton;
            case 9:
                return RightMouseButtonDown;
            case 10:
                return RightMouseButtonUp;
            default:
            return false;
        }
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

        Interact = Input.GetKeyDown(KeyCode.E);
        CloseUI = Input.GetKeyDown(KeyCode.Escape);

        DownArrow = Input.GetKeyDown(KeyCode.DownArrow);

        UpArrow = Input.GetKeyDown(KeyCode.UpArrow);

        LeftArrow = Input.GetKeyDown(KeyCode.LeftArrow);

        RightArrow = Input.GetKeyDown(KeyCode.RightArrow);

        openBlueprintUI = Input.GetKeyDown(KeyCode.B);

        LeftMouseButton = Input.GetKeyDown(KeyCode.Mouse0);

        RightMouseButtonDown = Input.GetKeyDown(KeyCode.Mouse1);

        RightMouseButtonUp = Input.GetKeyUp(KeyCode.Mouse1);
    }




    private void Update()
    {
        GetInput();
        ProcessMoveDirection(InputDirX, InputDirY);

    }
}