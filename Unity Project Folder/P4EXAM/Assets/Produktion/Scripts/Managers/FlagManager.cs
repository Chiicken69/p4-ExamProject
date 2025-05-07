using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum mode { Normal, Flag, Blueprint }
public class FlagManager : MonoBehaviour
{
    [SerializeField] private int _allowedFlagCount;
    [SerializeField] public GameObject FlagPrefab;
    public static FlagManager Instance;

    private Dictionary<Drone, List<Vector2>> droneFlags = new Dictionary<Drone, List<Vector2>>();  // To store flags for each drone

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to detect the clicked drone
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Drone clickedDrone = hit.collider.GetComponent<Drone>();
                if (clickedDrone != null)
                {
                    // Add flag for the clicked drone
                    AddFlagForDrone(clickedDrone, mousePos);
                }
            }
        }
    }

    // Adds a flag for a specific drone
    public void AddFlagForDrone(Drone drone, Vector2 flagPosition)
    {
        if (!droneFlags.ContainsKey(drone))
        {
            droneFlags[drone] = new List<Vector2>();
        }

        // Only add a new flag if it's not exceeding the allowed count
        if (droneFlags[drone].Count < _allowedFlagCount)
        {
            droneFlags[drone].Add(flagPosition);
            DisplayFlagsForDrone(drone);
        }
    }

    // Displays flags for a specific drone
    public void DisplayFlagsForDrone(Drone drone)
    {
        if (!droneFlags.ContainsKey(drone)) return;

        // Clear existing flags
        foreach (Transform child in drone.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate flags
        foreach (var flagPos in droneFlags[drone])
        {
            Instantiate(FlagPrefab, flagPos, Quaternion.identity, drone.transform);
        }
    }

    // Gets flags assigned to a specific drone
    public List<Vector2> GetFlagsForDrone(Drone drone)
    {
        if (droneFlags.ContainsKey(drone))
        {
            return droneFlags[drone];
        }
        return new List<Vector2>();
    }
}



