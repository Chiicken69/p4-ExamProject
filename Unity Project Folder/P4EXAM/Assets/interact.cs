using UnityEngine;

public class interact : MonoBehaviour
{
    private bool playerInFactoryTrigger = false;
    private bool playerInRadioTrigger = false;

    private bool _interact;

    public ArrowMinigame arrowMinigame;
    public RadioScript radioScript;
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private GameObject radioUI;
    private FactoryBase currentFactory;

    private void Update()
    {

        _interact = InputHandler.Instance.PassInputBoolValue(1);
        if ((playerInFactoryTrigger == true && _interact == true) && !minigameUI.activeInHierarchy)
        {
            //print("WHAT THE FUCK");
            arrowMinigame.StartMinigame();
            
        }
        if (currentFactory != null)
        {
            arrowMinigame.Checkstate(currentFactory);
        }
        if ((playerInRadioTrigger && _interact) && !radioUI.activeInHierarchy) 
        {
            radioUI.SetActive(true);
        } 
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Factory"))
        {
            playerInFactoryTrigger = true;

            // Attempt to get the FactoryBase component from the collided object
            FactoryBase factory = other.gameObject.GetComponent<FactoryBase>();
            if (factory != null)
            {
                // Store the reference for later use
                currentFactory = factory;
            }
            else
            {
                Debug.LogWarning("FactoryBase component not found on the collided object.");
            }


        }else if (other.gameObject.CompareTag("Radio"))
        {
            playerInRadioTrigger = true;  
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
       // print("omew");
        if (other.gameObject.CompareTag("Factory"))
        {
            playerInFactoryTrigger = false;
            currentFactory = null;
           // print("i have turned off");
        }
        if (other.gameObject.CompareTag("Radio"))
        {
            playerInRadioTrigger = false;
            radioUI.SetActive(false);
        }

        }
}
