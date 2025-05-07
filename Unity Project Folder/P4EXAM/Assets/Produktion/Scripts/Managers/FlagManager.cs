using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;
    public bool flagMode = false;

    public Drone selectedDrone;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flagMode = !flagMode;
            Debug.Log("Flag mode: " + flagMode);
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (flagMode && selectedDrone != null)
            {
                PlaceFlag(mousePos);
            }
            else
            {
                TrySelectDrone(mousePos);
            }
        }
    }

    void TrySelectDrone(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            Drone drone = hit.collider.GetComponent<Drone>();
            if (drone != null)
            {
                selectedDrone = drone;
                Debug.Log("Selected drone: " + drone.name);
            }
        }
    }

    void PlaceFlag(Vector2 pos)
    {
        var flags = selectedDrone.flagPoints;
        var flagObjs = selectedDrone.flagObjects;

        flags.Add(pos);
        if (flags.Count > selectedDrone.maxFlagCount)
        {
            flags.RemoveAt(0);
            Destroy(flagObjs[0]);
            flagObjs.RemoveAt(0);
        }

        GameObject flag = Instantiate(selectedDrone.flagPrefab, pos, Quaternion.identity);
        flagObjs.Add(flag);
        Debug.Log("Placed flag at: " + pos);
    }
}
