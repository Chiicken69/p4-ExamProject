
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

    int TempBuildingID;

    private Vector3 _mousePos;
    private bool _LeftMouseButton;

    [SerializeField] GameObject buildingButton;

    [SerializeField] private List<BuildingData> buildingPalette = new List<BuildingData>();

    private Dictionary<int, Sprite> idToSprite = new Dictionary<int, Sprite>();

    [SerializeField] GameObject building;

[System.Serializable]
    public class BuildingData
    {
        public int buildingID;
        public Sprite buildingSprite;
    }

    private void Start()
    {
        foreach (var data in buildingPalette)
        {
            if (!idToSprite.ContainsKey(data.buildingID))
            {
                idToSprite.Add(data.buildingID, data.buildingSprite);
            }
        }
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
        Sprite result = CalculateIdentification(buildingID);
        Debug.Log("Placing building: " + result);

        blueprintUI.SetActive(false);
        listenerAdded = false;
        _enteredBuildingMode = true;

        TempBuildingID = buildingID;

    }

    private void BuildingMode(int buildingID)
    {

        if (idToSprite.TryGetValue(buildingID, out Sprite sprite))
        {

            Vector3 gridPos = new Vector3(
            Mathf.RoundToInt(_mousePos.x),
            Mathf.RoundToInt(_mousePos.y), 0);
            for (int i = 0;i < 1;i++)
            {
                PlaceSpriteAtCell(gridPos, sprite, true);
            }
            
           
            if (_LeftMouseButton == true) 
            {
                PlaceSpriteAtCell(gridPos, sprite, false);
                _enteredBuildingMode = false;
            }
        }
        else
        {
            Debug.LogWarning("No sprite found for building ID: " + buildingID);
        }

    }
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
    void PlaceSpriteAtCell(Vector3 cellPos, Sprite sprite, bool preview)
    {

        switch (preview)
        {
            case true:
                if (buildingPreview == null) { 
                buildingPreview = new GameObject("PreviewBuilding");
                var previewSR = buildingPreview.AddComponent<SpriteRenderer>();
                previewSR.sprite = sprite;
                buildingPreview.transform.position = cellPos;
                }
                else
                {
                    buildingPreview.transform.position = cellPos;
                }
                break;

            case false:
                Destroy(buildingPreview);
                GameObject buildingPlaced = Instantiate(building);
                //var sr = building.AddComponent<SpriteRenderer>();
                buildingPlaced.GetComponent<SpriteRenderer>().sprite = sprite;
                buildingPlaced.transform.position = cellPos;
                break;
        }


    }
}
