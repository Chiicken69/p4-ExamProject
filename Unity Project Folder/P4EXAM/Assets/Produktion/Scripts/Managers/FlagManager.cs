using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;
public static bool _flagMode = false;

    public Drone selectedDrone;

    [SerializeField] Button FlagMangButton;
    Color PassiveColor = new Color(255, 255, 255, 1f);
    Color ToggledColor = new Color(255, 255, 255, 0.75f);

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _flagMode = !_flagMode;
            Debug.Log("FlagMode is now: " + _flagMode);

            if (!_flagMode)
            {
                print("SIGMAAAA");
                //ChangeButtonlook(Color.gray);
                FlagMangButton.GetComponent<UnityEngine.UI.Image>().color = PassiveColor ;
            }
            else
            {
                print("NOT VERY SIGMAA");
                FlagMangButton.GetComponent<UnityEngine.UI.Image>().color = ToggledColor;
                // ChangeButtonlook(Color.red);
            }
        }


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

       public void ChangeModeToFlagMode()
    {

        //_flagmode = !_flagmode;
        _flagMode = !_flagMode;
        Debug.Log("FlagMode is now: " + _flagMode);
    }

    public void ChangeButtonlook()
    {
        GameObject FlagMangButton = GameObject.FindGameObjectWithTag("FlagMangButton");
        if (!_flagMode)
        {
            print("SIGMAAAA");
            FlagMangButton.GetComponent<UnityEngine.UI.Image>().color = PassiveColor ;
            //ChangeButtonlook(Color.gray);
        }
        if (_flagMode)
        {
            print("NOT VERY SIGMAA");
            FlagMangButton.GetComponent<UnityEngine.UI.Image>().color = ToggledColor;
           // ChangeButtonlook(Color.red);
        }
    }

}
