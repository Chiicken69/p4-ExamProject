
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;



public class BlueprintBook : MonoBehaviour
{
    [SerializeField] private GameObject blueprintUI;

    SpriteRenderer previewSR;

    [SerializeField] private Text _placeingText;

    private bool _enteredBuildingMode = false;
    private bool listenerAdded = false;
    GameObject buildingPreview;
    [SerializeField] GridLayout _gridLayout;

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

    float _scroll;
    private float _minScoll = 0;
    private float _maxScoll;

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
        _maxScoll = building.Count;
    }




    // Update is called once per frame
    void Update()
    {

        GetKeyInfo();
        Openui();

        if (_enteredBuildingMode)
        {
            BuildingMode();
        }
        //Debug.Log("no sprite at id " + buildingID);






    }
 
    private void GetKeyInfo()
    {
            
        _mousePos = InputHandler.Instance.PassMousePosInWorld();
        _LeftMouseButton = InputHandler.Instance.PassInputBoolValue(8);

        _scroll = InputHandler.Instance.PassInputFloatValue();





    }
    private void GetScrollInfo()
    {

        Debug.Log(TempBuildingID);

        // Handle scrolling input and update buildingID
        if (_scroll > 0f)
        {
            TempBuildingID++;  // Scroll up
        }
        else if (_scroll < 0f)
        {
            TempBuildingID--;  // Scroll down
        }

        // Wrap around the buildingID within the valid range
        TempBuildingID = (int)Mathf.Repeat(TempBuildingID, building.Count);

    }
    private void Openui()
    {
        if (!_enteredBuildingMode)
        {
            blueprintUI.SetActive(true);

            if (!listenerAdded)
            {
                //buildingButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();

                buildingButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => PlaceBuilding());
                listenerAdded = true;

            }


        }
    
    }
    public void PlaceBuilding()
    {
 

        listenerAdded = false;
        _enteredBuildingMode = true;

        //Sprite result = CalculateIdentification(buildingID);

        blueprintUI.SetActive(false);
    }

    private void BuildingMode()
    {
        GetScrollInfo();


        Vector3 gridPos = _gridLayout.WorldToCell(_mousePos);
 

        if (TempBuildingID != 0)
        {
            _GhostPreview = building[TempBuildingID].GetComponent<FactoryBase>().idleSprite;
        }
        else if (TempBuildingID == 0)
        {
            _GhostPreview = null;
        }
        else
        {
            Debug.Log("no sprite at id" + TempBuildingID);
        }

        if (_GhostPreview != null)
        {
            PlaceSpriteAtCell(gridPos, true);
        }
        else if(_GhostPreview == null) { 
            Destroy(buildingPreview);
            _placeingText.text = "Placeing: Nothing";
        }
        if (_LeftMouseButton == true) 
            {
                PlaceSpriteAtCell(gridPos, false);
          
            }
        
    }

    void PlaceSpriteAtCell(Vector3 cellPos, bool preview)
    {
   
            switch (preview)
            {
                case true:

                if (buildingPreview == null)
                {
                    buildingPreview = new GameObject("PreviewBuilding");
                    previewSR = buildingPreview.AddComponent<SpriteRenderer>();
                    previewSR.sprite = _GhostPreview;

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
                    //oppsie

                    buildingPreview.GetComponent<SpriteRenderer>().sprite = _GhostPreview;
                    buildingPreview.transform.position = cellPos;
                    _placeingText.text = "Placeing: " + building[TempBuildingID].name;
                }

                break;

            case false:

                if (TempBuildingID != 0)
                {
                    var checkerComp = buildingPreview.GetComponent<CollisionChecker>();
                    if (checkerComp != null && checkerComp.isOverlapping)
                    {
                        Debug.Log("Cannot place building: Overlaps another structure.");
                        return;
                    }
                    _enteredBuildingMode = false;
                    _placeingText.text = "";
                    Destroy(buildingPreview);

                    GameObject buildingPlaced = Instantiate(building[TempBuildingID]);

                    buildingPlaced.transform.position = cellPos;
                    break;
                }
                else
                {
                    _placeingText.text = "";
                    _enteredBuildingMode = false;
                    break;
                }
        }
        

    }
}
