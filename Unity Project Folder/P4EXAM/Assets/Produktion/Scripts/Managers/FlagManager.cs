using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;
    [SerializeField] Image _blueprintBook;
    [SerializeField] Text _flagModeText;
    public static bool _flagMode = false;

    public List<Drone> selectedDrones = new List<Drone>();  // List of selected drones

    public GameObject highlight;
    private Vector2 _mousePos;

    [SerializeField] Button FlagMangButton;
    Color PassiveColor = new Color(255, 255, 255, 1f);
    Color ToggledColor = new Color(255, 255, 255, 0.75f);

public Sprite[] flagSprites; // Drag your "1", "2", "3" sprites into this in the Inspector

    private void Awake()
    {
        Instance = this;

        // Find the highlight child if not assigned
        if (highlight == null)
        {
            highlight = transform.Find("Highlight")?.gameObject;
        }

        if (highlight != null)
            highlight.SetActive(false); // Start disabled
    }

    void Update()
    {
        CheckModeForText();
        ChangeToFlagModeKeybind();
        MouseClickDetection();
        
        _mousePos = InputHandler.Instance.PassMousePosInWorld();

        CheckFlagsOnSelcDrone();
            HandleFlagAmmountChange(); // <- Add this line

        if (_flagMode == false)
        {
            unhighlight();
        }
    }

    void MouseClickDetection()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())  // Left-click detection
        {
            bool clickedDrone = TrySelectDrone(_mousePos); //returns true if drone is clicked, also adds clicked drones to selected drones list

            if (!clickedDrone && _flagMode && selectedDrones.Count > 0)  // If no drone clicked and flag mode is active
            {
                // If drones are selected, place flag
                foreach (var drone in selectedDrones)
                {
                    PlaceFlag(_mousePos, drone);

                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())  // Right-click to deselect
        {
            unhighlight();
        }
    }

    void unhighlight()
    {
        // Deselect all drones and turn off their highlights
        foreach (var drone in selectedDrones)
        {
            Transform highlight = drone.transform.Find("Highlight");
            if (highlight != null)
                highlight.gameObject.SetActive(false);
        }
        
        selectedDrones.Clear();  // Clear the selection list
    }
    bool TrySelectDrone(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            Drone drone = hit.collider.GetComponent<Drone>();
            if (drone != null)
            {
                // If Shift is held, add/remove the drone from the selected list
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (selectedDrones.Contains(drone))
                    {
                        // Deselect drone if already selected
                        selectedDrones.Remove(drone);
                        Transform highlight = drone.transform.Find("Highlight");
                        if (highlight != null)
                            highlight.gameObject.SetActive(false);
                    }
                    else
                    {
                        // Select drone
                        selectedDrones.Add(drone);
                        Transform highlight = drone.transform.Find("Highlight");
                        if (highlight != null)
                            highlight.gameObject.SetActive(true);
                    }
                }
                else
                {
                    // If Shift is not held, select only this drone and clear others
                    unhighlight();  // Deselect previous drones
                    selectedDrones.Add(drone);
                    Transform newHighlight = drone.transform.Find("Highlight");
                    if (newHighlight != null)
                        newHighlight.gameObject.SetActive(true);
                }

                //Debug.Log("Selected drones: " + string.Join(", ", selectedDrones.ConvertAll(d => d.name)));
                return true;
            }
        }

        return false;  // No drone was clicked
    }

    void PlaceFlag(Vector2 pos, Drone drone)
    {
        var flags = drone.flagPoints;
        var flagObjs = drone.flagObjects;



           flags.Add(pos);
        if (flags.Count > drone.maxFlagCount)
        {
            deleteLists();
            flags.Add(pos);
            drone.speed = 0;




        }

        GameObject flag = Instantiate(drone.flagPrefab, pos, Quaternion.identity);
        flagObjs.Add(flag);
        Debug.Log("Placed flag at: " + pos);

        // Set different sprite based on order
        int spriteIndex = flags.Count - 1;
        if (spriteIndex < flagSprites.Length)
        {
            flag.GetComponent<SpriteRenderer>().sprite = flagSprites[spriteIndex];
        }

        Debug.Log("Placed flag at: " + pos);
    }



 void HandleFlagAmmountChange()
{
    if (_flagMode && selectedDrones.Count > 0)
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var drone in selectedDrones)
            {
                deleteLists();
                drone.maxFlagCount = 2;
                Debug.Log("Set flag count to 2 for: " + drone.name);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (var drone in selectedDrones)
            {
                deleteLists();
                drone.maxFlagCount = 3;
                Debug.Log("Set flag count to 3 for: " + drone.name);
            }
        }
    }
}


 private void deleteLists()
{
    foreach (var drone in selectedDrones)
    {
        foreach (GameObject flagObj in drone.flagObjects)
        {
            Destroy(flagObj);
        }

        drone.flagObjects.Clear();
        drone.flagPoints.Clear();

        drone.StopAllCoroutines();
        drone.StartCoroutine(drone.PatrolFlags());
        drone.speed = 0;
    }
}




    void CheckFlagsOnSelcDrone()
    {
        var drones = DroneManager.Instance.drones;

        foreach (var droneObj in drones)
        {
            Drone droneScript = droneObj.GetComponent<Drone>();
            bool isSelected = selectedDrones.Contains(droneScript);
            ForEachFlagSprite(isSelected, droneScript.flagObjects);
        }
    }

    void ForEachFlagSprite(bool state, List<GameObject> Flags)
    {
        foreach (GameObject flag in Flags)
        {
            flag.GetComponent<SpriteRenderer>().enabled = state;
        }
    }

    public void ChangeModeToFlagMode()
    {
        _flagMode = !_flagMode;
        Debug.Log("FlagMode is now: " + _flagMode);
    }

    private void ChangeToFlagModeKeybind()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeModeToFlagMode();
            ChangeButtonlook();
        }
    }

    public void ChangeButtonlook()
    {
        GameObject FlagMangButton = GameObject.FindGameObjectWithTag("FlagMangButton");
        if (!_flagMode)
        {
            FlagMangButton.GetComponent<Image>().color = PassiveColor;
        }
        if (_flagMode)
        {
            FlagMangButton.GetComponent<Image>().color = ToggledColor;
        }
    }

    void CheckModeForText()
    {
        if (_flagMode)
        {
            _blueprintBook.enabled = false;
            _flagModeText.text = "LMB to select drones,\nhold leftShift to select multiple";
            if (selectedDrones.Count > 0)
            {
                _flagModeText.text = "LMB to move,\nLMB or RMB on drones deselect\npress 2 or 3 to select flag amount";
            }
        }
        else
        {
            _blueprintBook.enabled = true;
            _flagModeText.text = "";
        }
    }
}
