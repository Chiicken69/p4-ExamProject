using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;

    private float smoothTime = 0.2f;

    private Vector3 velocity;
    private Vector3 Pos;
    private Vector3 targetPos;


    private void FixedUpdate()
    {
        Pos = new Vector3(transform.position.x, transform.position.y, -10);
        targetPos = new Vector3(target.position.x, target.position.y, 0);

        transform.position = Vector3.SmoothDamp(Pos, targetPos, ref velocity, smoothTime);
    }
}
