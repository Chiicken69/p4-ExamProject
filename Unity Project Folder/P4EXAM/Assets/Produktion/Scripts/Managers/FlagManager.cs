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

    GameObject tempFlag;

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

        if (_flagMode == false)
        {
            unhighlight();
        }
    }

    void MouseClickDetection()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())  // Left-click detection
        {
            bool clickedDrone = TrySelectDrone(_mousePos);

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
            flags.RemoveAt(0);
            Destroy(flagObjs[0]);
            flagObjs.RemoveAt(0);
        }

        GameObject flag = Instantiate(drone.flagPrefab, pos, Quaternion.identity);
        flagObjs.Add(flag);
        Debug.Log("Placed flag at: " + pos);
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
                _flagModeText.text = "LMB to move,\nLMB or RMB on drones deselect";
            }
        }
        else
        {
            _blueprintBook.enabled = true;
            _flagModeText.text = "";
        }
    }
}
