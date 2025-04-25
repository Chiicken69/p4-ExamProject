using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public bool isOverlapping = false;

    private void OnTriggerStay2D(Collider2D other)
    {
       // Debug.Log("Trigger Enter with: " + other.name);
        if (other.CompareTag("Factory") || other.CompareTag("Player"))
        {
            isOverlapping = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Trigger Exit with: " + other.name);
        if (other.CompareTag("Factory") || other.CompareTag("Player"))
        {
            isOverlapping = false;
        }
    }
}
