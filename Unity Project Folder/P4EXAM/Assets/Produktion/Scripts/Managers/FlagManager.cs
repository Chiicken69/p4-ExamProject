using UnityEngine;

public class FlagManager : MonoBehaviour
{
    private Vector3 MouseWorldPos;



    private void Update()
    {
        MouseWorldPos = InputHandler.Instance.PassMousePosInWorld();



    }
}
