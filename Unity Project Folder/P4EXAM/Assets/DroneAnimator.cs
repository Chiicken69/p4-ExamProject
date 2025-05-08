using UnityEngine;

public class DroneAnimator : MonoBehaviour
{
private Animator _animator;
     Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 lastPosition;
    bool isFacingRight = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;
        float deltaX = currentPosition.x - lastPosition.x;

        if (Mathf.Abs(deltaX) > 0.01f)
        {
            bool facingRightNow = deltaX > 0f;

            if (facingRightNow != isFacingRight)
            {
                isFacingRight = facingRightNow;
                animator.SetBool("IsFacingRight", isFacingRight);
            }

            // Optional: flip sprite visually
            spriteRenderer.flipX = !isFacingRight;
        }

        lastPosition = currentPosition;
    }
}
