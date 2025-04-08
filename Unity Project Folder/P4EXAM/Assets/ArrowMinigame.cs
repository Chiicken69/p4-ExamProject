using UnityEngine;

public class ArrowMinigame : MonoBehaviour
{
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private CameraZoom playerZoom;

    private bool _interact;
    private bool _closeUI;

    private void Update()
    {
        GetKeyInfo();
        StartMinigame();
    }
    private void GetKeyInfo()
    {
        _interact = InputHandler.Instance.PassInputBoolValue(1);
        _closeUI = InputHandler.Instance.PassInputBoolValue(2);
    }

   
    private void StartMinigame()
    {
        if (_interact == true)
        {
            minigameUI.SetActive(true);
            playerMovement.enabled = false;
            playerZoom.enabled = false;
            CalculateArrows(true);
            Debug.Log("Opened arrowui");
        }
        else if (_closeUI == true && minigameUI.activeInHierarchy)
        {
            minigameUI.SetActive(false);
            playerMovement.enabled = true;
            playerZoom.enabled = true;
            CalculateArrows(false);
            Debug.Log("closed arrow ui");
        }
    }
    private void CalculateArrows(bool gameState)
    {
    }

}
