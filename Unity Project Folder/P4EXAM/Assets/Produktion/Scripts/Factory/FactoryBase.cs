using UnityEngine;
using System.Collections.Generic;
using System.Collections;



enum FactoryState {Idle, Building, Processing, Crafting};




public class FactoryBase : MonoBehaviour
{



    FactoryState state;
    private ItemBase.ItemType ListOfItems;
    List<GameObject> OutputInventory;
    List<GameObject> InputInventory;

    [Header("Ingredients")]
    private bool _readyToCraft;
    // Ingredient amount should be at same index as ingredient list.
    [SerializeField] List<ItemBase.ItemType> _ingredientList;
    [SerializeField] List<int> _ingredientAmountForCraft;
    [SerializeField] ItemBase.ItemType _itemOutputType;

    [Header("Internal mechanics")]
    [SerializeField] private float _craftingTime;






    public void InitializeFactory()
    {
        state = FactoryState.Building;
    }

    public void ClearOutputInventory()
    {
        OutputInventory.Clear();   
    }

    public void CreateOutput()
    {
        GameObject ObjectToAdd = new GameObject(_itemOutputType.ToString());
        ObjectToAdd.AddComponent<ItemBase>();
        OutputInventory.Add(ObjectToAdd); 
    }

    public void AddItemToInventory(GameObject ObjectToAdd)
    {
        InputInventory.Add(ObjectToAdd);
    }

    public GameObject TakeItemFromOutputInventory()
    {
        int i = OutputInventory.Count;
        GameObject TemporaryObject = OutputInventory[i];
        OutputInventory.RemoveAt(i);
        return TemporaryObject;
    }

    public void ClearInputInventory()
    {
        InputInventory.Clear();
    }

IEnumerator craft()
    {
        ClearInputInventory();
        yield return new WaitForSeconds(_craftingTime);

    }

    private void CheckForCraftingPossible()
    {
        if (_readyToCraft)
        {
            craft();
            _readyToCraft = false;
        }
    }

    private bool ItemsFilled()
    {
        int x = 0;
        int p = 0;

        for (int i = 0; i < _ingredientList.Count; i++)
        {
            x++;

            for (int k = 0; k < InputInventory.Count; k++)
            {
                if (InputInventory[i].name == _ingredientList[k].ToString())
                {
                    p++;
                }

            }

           
        }
        return true;
    }

    









}
