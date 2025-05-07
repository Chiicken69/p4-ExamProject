using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public bool isOverlapping = false;

    private int overlapCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsBlockingTag(other.tag))
            overlapCount++;
        UpdateOverlapState();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsBlockingTag(other.tag))
            overlapCount--;
        UpdateOverlapState();
    }

    private void UpdateOverlapState()
    {
        isOverlapping = overlapCount > 0;
    }

    private bool IsBlockingTag(string tag)
    {
        return tag == "Factory" || tag == "Player" || tag == "Building" || tag == "Radio";
    }
}

