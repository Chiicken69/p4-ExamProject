using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;



public enum FactoryState {Idle, Building, Processing, Crafting};




public class FactoryBase : MonoBehaviour, Ifactory
{
    public GameObject ReturnFactory() { return this.gameObject; }
    private Slider _timerSlider;
    private Canvas _canvas;
    private TextMeshProUGUI _textMeshProUGUI;


    public FactoryState state;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject droneObjReference;

    [Header("Inventory sneak peek")]
    [SerializeField] private List<GameObject> outputInventory;
   [SerializeField] private List<GameObject> inputInventory;

    [Header("Sprites for states")]

   
    [SerializeField] Sprite buildingSprite;
    [SerializeField] public Sprite idleSprite;
    [SerializeField] Sprite craftingSprite;
    [SerializeField] Sprite processingSprite;

    Sprite _currentSprite;

    [Header("For looking at, dont change in editor")]
    [SerializeField] private float _tempCraftingTime;
    [SerializeField] public float speedIncreasePercentage;

    [Header("Internal mechanics")]

    [SerializeField] private float _craftingTime;
    [SerializeField] private float _secondsToDecreaseSpeed;
    private float internalTimer;
    private bool _readyToCraft = true;


    [Header("Item recipe, and output type")]

    // Ingredient amount should be at same index as ingredient list.
    [SerializeField] List<ItemBase.ItemType> _ingredientList;
    [SerializeField] List<int> _ingredientAmountForCraft;
    [SerializeField] GameObject _itemOutputType;
     

    [Header("Factory building recipe")]

    [SerializeField] List<ItemBase.ItemType> _itemListToCraftFactory;
    [SerializeField] List<int> _itemAmountToCraftFactory;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = FactoryState.Building;
        _tempCraftingTime = _craftingTime;
        internalTimer = _secondsToDecreaseSpeed;
        _canvas = GetComponentInChildren<Canvas>();
       _timerSlider = _canvas.GetComponentInChildren<Slider>();
        _textMeshProUGUI = _canvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Update()
    {
        DecreaseCraftingSpeed();
        // temporary testing code
        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreaseCraftingSpeed();
        }

        switch (state)
        {
            case FactoryState.Idle:
                CheckForCraftingPossible();
                break;
            case FactoryState.Building:
                //building logic
                break;
            case FactoryState.Processing:
                //processing logic
                break;
            case FactoryState.Crafting:
                //crafting logic
                break;
            default:
                break;
        }
        print(state);
        SwitchSprite(state);
        DisplayFactoryUI();

    }

    private void SwitchSprite(FactoryState state)
    {
        switch (state)
        {
            case FactoryState.Idle:
                _currentSprite = idleSprite;
                break;
            case FactoryState.Building:
                _currentSprite = buildingSprite;
                break;
            case FactoryState.Processing:
                _currentSprite = processingSprite;
                break;
            case FactoryState.Crafting:
                _currentSprite = craftingSprite;
                break;
            default:
                _currentSprite = idleSprite;
                break;
        }
        spriteRenderer.sprite = _currentSprite;
        Debug.Log(_currentSprite.ToString());
    }



    private void Start()
    {
        InitializeFactory();
        StartCoroutine(BuildFactory());
    }

    public IEnumerator BuildFactory()
    {
        while (true)
        {
            
            if (ItemsFilled(_itemListToCraftFactory, inputInventory, _itemAmountToCraftFactory))
            {
                
                ClearinputInventory();
                state = FactoryState.Idle;
                StopCoroutine(BuildFactory());
                break;
            }
            yield return null;
        }

        

    }

    public void InitializeFactory()
    {

        FactoryManager.Instance.AddFactory(this.gameObject);
        state = FactoryState.Building;
        
    }

    public void ClearOutputInventory()
    {
        outputInventory.Clear();   
    }
    [ContextMenu("Create Output")]
    public void CreateOutput()
    {
        GameObject ObjectToAdd = _itemOutputType;
        //ObjectToAdd.AddComponent<ItemBase>();
        

        if (_itemOutputType == droneObjReference )
        {
            Debug.LogWarning("drone obj is in here!!!");
            DroneManager.Instance.SpawnDrone(this.gameObject);
        }
        outputInventory.Add(ObjectToAdd); 
    }

    public void AddItemToInventory(GameObject ObjectToAdd)
    {
        if (ObjectToAdd == null)
        {
            print("Object to add was null");
            return;
            
        } else if (CheckAgainstRecipe(ObjectToAdd))
        {
            print("GAYYYYYYY");
        }


        inputInventory.Add(ObjectToAdd);
    }
    /// <summary>
    /// Returns true if item is in recipe, returns false if not
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public bool CheckAgainstRecipe(GameObject gameObject)
    {
        
        ItemBase ObjItem = gameObject.GetComponent<ItemBase>();
        foreach (var item in _ingredientList)
        {
            if (ObjItem.type == item)
            {
                return true;
            }


        }
        return false;

    }


    // other scripts call this to take items
    public GameObject TakeItemFromOutputInventory()
    {
        if (outputInventory.Count <= 0)
        {
            return null;
        }
        int i = outputInventory.Count;

        GameObject TemporaryObject = outputInventory[i-1];
        outputInventory.RemoveAt(i-1);
        return TemporaryObject;
    }

    public void ClearinputInventory()
    {
        inputInventory.Clear();
    }

    IEnumerator Craft()
    {
        // for each ingredient type...
        for (int i = 0; i < _ingredientList.Count; i++)
        {
            var neededType = _ingredientList[i];
            var amountNeeded = _ingredientAmountForCraft[i];
            int k = 0;

            // collect matching GameObjects to destroy
            var toRemove = new List<GameObject>();

            foreach (var go in inputInventory)
            {
                if (k >= amountNeeded)
                    break;

                var currentItem = go.GetComponent<ItemBase>();
                if (currentItem.type == neededType)
                {
                    toRemove.Add(go);
                    k++;
                }
            }

            // now actually remove them
            foreach (var go in toRemove)
                inputInventory.Remove(go);
        }
        // ClearinputInventory();
        print("crafting started");
        state = FactoryState.Crafting;
        _tempCraftingTime = _craftingTime;

        while (_tempCraftingTime >= 0)
        {



            _tempCraftingTime -= Time.deltaTime * (1f + (speedIncreasePercentage / 100f));







            yield return null;

            if (_tempCraftingTime <= 0)
            {
                CreateOutput();
                _readyToCraft = true;
                state = FactoryState.Idle;
                StopCoroutine(Craft());
                //break;
            }

        }






    }

    public void CheckForCraftingPossible()
    { // not done

        
        if (_readyToCraft)
        {
            print("test");
            if (ItemsFilled(_ingredientList, inputInventory, _ingredientAmountForCraft))
            {
                print("Check is true!");
                StartCoroutine(Craft());
                _readyToCraft = false;
            }
           
        }
    }

    public bool ItemsFilled(List<ItemBase.ItemType> itemlist, List<GameObject> inventory, List<int> itemAmount) // hashmap when? :pleading emoji:
    {
        int i = 0;
        int k = 0;
        int TrueReturns = 0;

        foreach (var ingredient in itemlist)
        {
            foreach (var item in inventory)
            {
                ItemBase tempItemBase = item.GetComponent<ItemBase>();
                if (tempItemBase.type == ingredient) k++;


            }
            if (k >= itemAmount[i])
            {
                TrueReturns++;
            }
            i++;
            k = 0;
        }
        if (TrueReturns >= itemlist.Count)
        {
            return true;
          
        } else
        // state = FactoryState.Idle;
         return false;

    }
    
    /// <summary>
    /// Decreases the crafting time of the factory the script is on. Call this from other scripts
    /// </summary>
    public void IncreaseCraftingSpeed()
    {
        //works but needs to be adjusted later for actual gameplay, 
        // _CraftingBoosterValue++;
        // _tempCraftingTime /= _CraftingBoosterValue;
        if (state != FactoryState.Building)
        {
            speedIncreasePercentage += 400;
        }

        


        print("Decrease temporary crafting time function was called, this should only be called once");
    }
   

   private void DecreaseCraftingSpeed()
    {
        internalTimer -= Time.deltaTime;
        if (internalTimer <= 0 && speedIncreasePercentage > 0)
        {
            speedIncreasePercentage--;
            internalTimer = _secondsToDecreaseSpeed;
        }
    }
    public void DisplayFactoryUI()
    {
        DisplayTimer();
        DisplayRecipeText();
    }

    public void DisplayTimer()
    {
        // intialize timer values
        if (state == FactoryState.Building)
        {
            //innefecient as hell
            int i = 0;
            foreach (var item in _itemAmountToCraftFactory)
            {
                i += item;
            }
            float TotalItems = (float)i;

            _timerSlider.maxValue = TotalItems;
            print(TotalItems);
            _timerSlider.minValue = 0;

            float ItemsInFactory = inputInventory.Count;
            _timerSlider.value = ItemsInFactory;

        }
        else
        {
              _timerSlider.maxValue = _craftingTime;
            _timerSlider.minValue = 0;

            _timerSlider.value = _tempCraftingTime;

        }
       
        

    }
    private void DisplayRecipeText()
    {
        _textMeshProUGUI.text = GenerateRecipeText();
    }

    private string GenerateRecipeText()
    {
        string text ="";
        if (state == FactoryState.Building)
        {
            for (global::System.Int32 i = 0; i < _itemListToCraftFactory.Count; i++)
            {
                text += _itemListToCraftFactory[i] +": " + CheckinputInventory(_itemListToCraftFactory[i]) +"/" + _itemAmountToCraftFactory[i] + "\n";
                
            }
        }
        else
        {
            for (global::System.Int32 i = 0; i < _ingredientList.Count; i++)
            {
                text += _ingredientList[i] + ": " + CheckinputInventory(_ingredientList[i]) + "/" + _ingredientAmountForCraft[i] + "\n";

            }
        }
        return text;

    }

    private int CheckinputInventory(ItemBase.ItemType TypeToCheck)
    {
        int i = 0;
        foreach (var item in inputInventory)
        {
            ItemBase itembase = item.GetComponent<ItemBase>();
            
            if (itembase.type == TypeToCheck)
            {
                i++;
            }
        }

        return i;
    }




}
