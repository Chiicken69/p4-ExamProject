using UnityEngine;

public class BuildingDeleter : MonoBehaviour
{
    private Vector3 _mousePos;
    private bool _rightMouseButtonDown;
    private bool _rightMouseButtonUp;
    private bool onFactory;
    private bool inFactory;
    private Collider2D hit;

    private float rightClickStartTime = 0f;
    private bool isRightClickHeld = false;
    private bool hasTriggered = false;

    private SpriteRenderer currentRenderer = null;
    private Color originalColor;
    private float colorLerpSpeed = 0.5f;

    private void Update()
    {
        GetKeyInfo();

        if (inFactory && onFactory)
        {
            CheckIfBoolsTrue();
        }

        HandleColorLerp();
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

            var newRenderer = hit.GetComponent<SpriteRenderer>();
            if (newRenderer != currentRenderer)
            {
                // Cache the new renderer and its original color
                currentRenderer = newRenderer;
                originalColor = currentRenderer.color;
            }
        }
        else
        {
            onFactory = false;
        }

        inFactory = other.gameObject.CompareTag("Factory");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (currentRenderer != null)
        {
            currentRenderer.color = originalColor;
        }

        // Clear state when leaving
        onFactory = false;
        inFactory = false;
        hit = null;
        currentRenderer = null;
        isRightClickHeld = false;
        hasTriggered = false;
    }
    private void CheckIfBoolsTrue()
    {
        if (inFactory && onFactory)
        {
            if (_rightMouseButtonDown)
            {
                rightClickStartTime = Time.time;
                isRightClickHeld = true;
                hasTriggered = false;

            }

            if (_rightMouseButtonUp)
            {
                isRightClickHeld = false;
                hasTriggered = false;

                // Reset color when released
                if (currentRenderer != null)
                {
                    currentRenderer.color = originalColor;
                }
            }

            if (isRightClickHeld && !hasTriggered)
            {
                float heldDuration = Time.time - rightClickStartTime;

                if (heldDuration >= 3f && inFactory && onFactory)
                {
                    hasTriggered = true;
                    Destroy(hit.gameObject);
                    currentRenderer = null; // clear after destroy
                }
            }
        }
    }

    private void HandleColorLerp()
    {
        if (isRightClickHeld && currentRenderer != null)
        {
            Color targetColor = Color.red;
            currentRenderer.color = Color.Lerp(currentRenderer.color, targetColor, Time.deltaTime * colorLerpSpeed);
        }
    }
}
