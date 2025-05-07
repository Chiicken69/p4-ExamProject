using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left-click
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Drone clickedDrone = hit.collider.GetComponent<Drone>();
                if (clickedDrone != null)
                {
                    // Add a flag at the clicked position for the selected drone
                    FlagManager.Instance.AddFlagForDrone(clickedDrone, mousePos);
                }
            }
        }
    }
}