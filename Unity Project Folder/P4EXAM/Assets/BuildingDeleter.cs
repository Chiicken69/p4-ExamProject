using UnityEngine;


public class BuildingDeleter : MonoBehaviour
{

    private Vector3 _mousePos;
    private bool _rightMouseButtonDown;
    private bool _rightMouseButtonUp;
    private bool onFactory;
    private bool inFactory;
    Collider2D hit;

    private float rightClickStartTime = 0f;
    private bool isRightClickHeld = false;
    private bool hasTriggered = false;


   
    private void Update()
    {
        GetKeyInfo();
        if (inFactory && onFactory) { 
        CheckIfBoolsTrue();
        }
    }
    private void GetKeyInfo()
    {
        _mousePos = InputHandler.Instance.PassMousePosInWorld();
        _rightMouseButtonDown = InputHandler.Instance.PassInputBoolValue(9);
        _rightMouseButtonUp = InputHandler.Instance.PassInputBoolValue(10);
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        hit = Physics2D.OverlapPoint(_mousePos);


        if (hit != null && hit.CompareTag("Factory"))
            {
                onFactory = true;
            }
            else
            {
                onFactory = false;
            }

            if (other.gameObject.CompareTag("Factory"))
            {
                inFactory = true;
            }
            else { inFactory = false; }
        
    }
    void OnTriggerExit2D(Collider2D other) {
        if (hit != null && hit.CompareTag("Factory"))
        {
            onFactory = false;
        }
     
    }
    private void CheckIfBoolsTrue()
    {
        if (_rightMouseButtonDown)
        {
            rightClickStartTime = Time.time;
            isRightClickHeld = true;
            hasTriggered = false;
    
        }

        // When right-click is released, reset the state
        if (_rightMouseButtonUp)
        {
            isRightClickHeld = false;
            hasTriggered = false;

        }

        // Check if the right mouse button is held for more than 3 seconds
        if (isRightClickHeld && !hasTriggered)
        {
            float heldDuration = Time.time - rightClickStartTime;

            if (heldDuration >= 3f && inFactory && onFactory)
            {
                hasTriggered = true;
                Destroy(hit.gameObject);
            }
        }
    }
}   