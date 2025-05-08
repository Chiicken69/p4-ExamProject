using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;

    [SerializeField] Image _blueprintBook;
    [SerializeField] Text _flagModeText;
    public static bool _flagMode = false;

    public Drone selectedDrone;

    public GameObject highlight;


    [SerializeField] Button FlagMangButton;
    Color PassiveColor = new Color(255, 255, 255, 1f);
    Color ToggledColor = new Color(255, 255, 255, 0.75f);

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
    }

    void MouseClickDetection()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_flagMode && selectedDrone != null)
            {
                PlaceFlag(mousePos);
            }
            else
            {
                TrySelectDrone(mousePos);
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Deselect drone and turn off its highlight
            if (selectedDrone != null)
            {
                Transform highlight = selectedDrone.transform.Find("Highlight");
                if (highlight != null)
                    highlight.gameObject.SetActive(false);

                selectedDrone = null;
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
                // Deselect previous
                if (selectedDrone != null)
                {
                    Transform prevHighlight = selectedDrone.transform.Find("Highlight");
                    if (prevHighlight != null)
                        prevHighlight.gameObject.SetActive(false);
                }

                // Select new
                selectedDrone = drone;

                Transform newHighlight = selectedDrone.transform.Find("Highlight");
                if (newHighlight != null)
                    newHighlight.gameObject.SetActive(true);

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

    public void ChangeModeToFlagMode()
    {

        //_flagmode = !_flagmode;
        _flagMode = !_flagMode;
        Debug.Log("FlagMode is now: " + _flagMode);
    }

    private void ChangeToFlagModeKeybind(){
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
            print("SIGMAAAA");
            FlagMangButton.GetComponent<Image>().color = PassiveColor;
            //ChangeButtonlook(Color.gray);
        }
        if (_flagMode)
        {
            print("NOT VERY SIGMAA");
            FlagMangButton.GetComponent<Image>().color = ToggledColor;
            // ChangeButtonlook(Color.red);
        }
    }

    void CheckModeForText()
    {
        if (_flagMode)
        {
            _blueprintBook.enabled = false;
            _flagModeText.text = "LMB to select a drone";
            if (selectedDrone != null)
            {
                _flagModeText.text = "LMB to move, RMB to deselect";
            }
        }
        else
        {
            _blueprintBook.enabled = true;
            _flagModeText.text = "";
        }

    }

}
