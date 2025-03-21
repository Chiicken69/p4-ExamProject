using UnityEngine;
using System.Collections.Generic;



enum FactoryState {Idle, Building, Processing, Crafting};




public class FactoryBase : MonoBehaviour
{



    FactoryState state;
    private ItemBase.ItemType ListOfItems;
    List<GameObject> OutputInventory;






public void InitializeFactory()
    {
        state = FactoryState.Building;
       

    }

    public void CreateOutputInventory()
    {
        OutputInventory.Clear();   
    }

    public void CreateInputInventory()
    {

    }

    









}
