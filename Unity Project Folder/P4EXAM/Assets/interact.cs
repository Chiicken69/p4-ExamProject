using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class interact : MonoBehaviour
{
    private bool playerInFactoryTrigger = false;
    private bool playerInRadioTrigger = false;

    private bool _interact;

    public ArrowMinigame arrowMinigame;
    public RadioScript radioScript;
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private GameObject radioUI;
    private GameObject currentFactory;
    private GameObject[] factoryTexts;

    private void Update()
    {

        _interact = InputHandler.Instance.PassInputBoolValue(1);
        if ((playerInFactoryTrigger == true && _interact == true) && !minigameUI.activeInHierarchy)
        {
            //print("WHAT THE FUCK");
            AudioManager.Instance.PlaySFXArrayRandom("ButtonClicks");
            arrowMinigame.StartMinigame();
            
        }
        if (currentFactory != null)
        {
            arrowMinigame.Checkstate(currentFactory);
        }
        if ((playerInRadioTrigger && _interact) && !radioUI.activeInHierarchy) 
        {
            AudioManager.Instance.PlaySFX("ButtonClick");
            radioUI.SetActive(true);
        } 
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Factory"))
        {
            playerInFactoryTrigger = true;

            // Attempt to get the FactoryBase component from the collided object
            GameObject factory = other.gameObject;
            if (factory != null)
            {
                // Store the reference for later use
                currentFactory = factory;
                Image[] allTextMeshes = other.GetComponentsInChildren<Image>();

                foreach (var img in allTextMeshes)
                {
                    if (img.gameObject.name == "PresssE")
                    {
                        img.enabled = true;
                    }
                }



            }
            else
            {
                Debug.LogWarning("FactoryBase component not found on the collided object.");
            }


        }else if (other.gameObject.CompareTag("Radio"))
        {
            playerInRadioTrigger = true;
            SpriteRenderer[] allTextMeshes = other.GetComponentsInChildren<SpriteRenderer>();

            foreach (var obj in allTextMeshes)
            {
                if (obj.gameObject.name == "PresssE")
                {
                    obj.enabled = true;
                }
            }
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

            Image[] allTextMeshes = other.GetComponentsInChildren<Image>();

            foreach (var img in allTextMeshes)
            {
                if (img.gameObject.name == "PresssE")
                {
                    img.enabled = false;
                }
            }



        }
        if (other.gameObject.CompareTag("Radio"))
        {
            playerInRadioTrigger = false;
            radioUI.SetActive(false);

            SpriteRenderer[] allSR = other.GetComponentsInChildren<SpriteRenderer>();

            foreach (var SR in allSR)
            {
                if (SR.gameObject.name == "PresssE")
                {
                    SR.enabled = false;
                }
            }
        }

        }
}
