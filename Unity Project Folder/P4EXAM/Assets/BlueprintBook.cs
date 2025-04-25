
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class BlueprintBook : MonoBehaviour
{
    [SerializeField] private GameObject blueprintUI;

    private bool _openBlueprintUI;
    private bool _closeUI;
    private bool _enteredBuildingMode = false;
    private bool listenerAdded = false;
    GameObject buildingPreview;

    //here is where u determin scale for buildings
    int Scaleforsprite = 1;

    int TempBuildingID;

    private Vector3 _mousePos;
    private bool _LeftMouseButton;

    [SerializeField] GameObject buildingButton;

    [SerializeField] private List<BuildingData> buildingPalette = new List<BuildingData>();

    //private Dictionary <int, Sprite> idToSprite = new Dictionary<int, Sprite>();

    [SerializeField] private List<GameObject> building;

    Sprite _GhostPreview;

    [System.Serializable]
    public class BuildingData
    {
        public int buildingID;
        public Sprite buildingSprite;
    }

    private void Start()
    {
        /*
        foreach (var data in buildingPalette)
        {
            if (!idToSprite.ContainsKey(data.buildingID))
            {
                idToSprite.Add(data.buildingID, data.buildingSprite);
            }
        }
        */

    }

    // Update is called once per frame
    void Update()
    {

        GetKeyInfo();
        Openui();

        if (_enteredBuildingMode)
        {
            BuildingMode(TempBuildingID);
        }
       
    }

    private void GetKeyInfo()
    {
        _closeUI = InputHandler.Instance.PassInputBoolValue(2);
        _openBlueprintUI = InputHandler.Instance.PassInputBoolValue(7);
        _mousePos = InputHandler.Instance.PassMousePosInWorld();
        _LeftMouseButton = InputHandler.Instance.PassInputBoolValue(8);



    }

    private void Openui()
    {
        if (_openBlueprintUI == true && !blueprintUI.activeInHierarchy)
        {
            blueprintUI.SetActive(true);

            if (!listenerAdded)
            {
                buildingButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();

                Identification idComponent = buildingButton.GetComponent<Identification>();
                int id = idComponent.buildingID;

                buildingButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => PlaceBuilding(id));
                listenerAdded = true;

            }


        }
        else if (_closeUI == true)
        {
            blueprintUI.SetActive(false);
            listenerAdded = false;
        }
    }
    public void PlaceBuilding(int buildingID)
    {
        blueprintUI.SetActive(false);
        listenerAdded = false;
        _enteredBuildingMode = true;

        TempBuildingID = buildingID;

        if (_GhostPreview == null)
        {
            _GhostPreview = building[TempBuildingID].GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            Debug.LogWarning("No sprite found for building ID: " + buildingID);
        }

        //Sprite result = CalculateIdentification(buildingID);


        Debug.Log("Placing building: " + _GhostPreview);

    }

    private void BuildingMode(int buildingID)
    {

        else
        {

            Vector3 gridPos = new Vector3(
            Mathf.RoundToInt(_mousePos.x),
            Mathf.RoundToInt(_mousePos.y), 0);
 

            PlaceSpriteAtCell(gridPos, _GhostPreview, true);

            
           
            if (_LeftMouseButton == true) 
            {
                PlaceSpriteAtCell(gridPos, _GhostPreview, false);
          
            }
        }
 

    }
    /*
    public Sprite CalculateIdentification(int buildingID)
    {
        foreach (var item in idToSprite)
        {
            if (buildingID == item.Key)
            {
                return item.Value;
            }

        }
        return null;
    }
    
    public GameObject PlaceWhatBuilding(Sprite Building)
    {
        //goofy aaa way of checking what building to place based on sprite
        switch (Building.name)
        {
            case "Wire Factory":

                print("made wire");
                return buildingPlaced;

            case "Spool Factory":
                GameObject buildingPlaced = Instantiate(building[TempBuildingID]);
                print("made spool");
                return buildingPlaced;
            default:
                print("Incorrect intelligence level.");
                return null;
        }
    }
    */

    void PlaceSpriteAtCell(Vector3 cellPos, Sprite sprite, bool preview)
    {
        if (sprite != null)
        {
            switch (preview)
            {
                case true:

                    if (buildingPreview == null)
                    {
                        buildingPreview = new GameObject("PreviewBuilding");
                        var previewSR = buildingPreview.AddComponent<SpriteRenderer>();
                        previewSR.sprite = sprite;

                        var boxCollider = buildingPreview.AddComponent<BoxCollider2D>();
                        boxCollider.size = previewSR.sprite.bounds.size;
                        buildingPreview.GetComponent<BoxCollider2D>().isTrigger = true;


                        // Add Rigidbody2D
                        var rb = buildingPreview.AddComponent<Rigidbody2D>();
                        rb.bodyType = RigidbodyType2D.Kinematic;
                        rb.gravityScale = 0;
                        rb.simulated = true;

                        var checker = buildingPreview.AddComponent<CollisionChecker>();

                        //checks if building id is for the factory check the scipt under player to see all id's
                        if (TempBuildingID <= 2)
                        { Scaleforsprite = 3; }
                        else { Scaleforsprite = 1; }

                        buildingPreview.transform.localScale = new Vector3(Scaleforsprite, Scaleforsprite, Scaleforsprite);
                        buildingPreview.transform.position = cellPos;
                    }
                    else
                    {
                        buildingPreview.transform.position = cellPos;
                    }

                    break;

                case false:

                    var checkerComp = buildingPreview.GetComponent<CollisionChecker>();
                    if (checkerComp != null && checkerComp.isOverlapping)
                    {
                        Debug.Log("Cannot place building: Overlaps another structure.");
                        return;
                    }
                    _enteredBuildingMode = false;
                    Destroy(buildingPreview);

                    GameObject buildingPlaced = Instantiate(building[TempBuildingID]);

                    buildingPlaced.transform.position = cellPos;
                    break;
            }
        }

    }
}
