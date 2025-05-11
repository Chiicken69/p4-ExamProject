
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ArrowMinigame : MonoBehaviour
{
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private GameObject arrow;
    [SerializeField] private TextMeshProUGUI factoryMultText;

    [SerializeField] private Transform Parent;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private CameraZoom playerZoom;
    [SerializeField] private List<Sprite> spriteList;

    List<GameObject> cloneArrowList = new List<GameObject>();
    public List<DirectionName> directionList = new List<DirectionName>();

    //private bool _interact;
    private bool _closeUI;
    private bool RightArrow;
    private bool UpArrow;
    private bool DownArrow;
    private bool LeftArrow;

    public bool minigameTutorielDone = false;

    private bool currentRoundPlaying = false;
    private Sprite arrowImage;

//    int direction;
  //  string result;
    public enum DirectionName { Up, Left, Right, Down, None }

    private int _arrowAmount = 5;
    public int currentArrowIndex = 0;



    
    private void Update()
    {
        GetKeyInfo();

    }
    private void GetKeyInfo()
    {
        //_interact = InputHandler.Instance.PassInputBoolValue(1);
        _closeUI = InputHandler.Instance.PassInputBoolValue(2);
        RightArrow = InputHandler.Instance.PassInputBoolValue(3);
        LeftArrow = InputHandler.Instance.PassInputBoolValue(4);
        DownArrow = InputHandler.Instance.PassInputBoolValue(5);
        UpArrow = InputHandler.Instance.PassInputBoolValue(6);

    }

    public void Checkstate(GameObject currentFactory)
    {
    
        if (_closeUI == true && minigameUI.activeInHierarchy)
        {
            StopMinigame();
            AudioManager.Instance.PlaySFXArrayRandom("ButtonClicks");
        }
        if (currentRoundPlaying == true)
        {
            RunRound(currentFactory.GetComponent<FactoryBase>());
        }
    }


    public void StartMinigame()
    {
        minigameUI.SetActive(true);
        playerMovement.enabled = false;
        playerZoom.enabled = false;
        CalculateArrows(true);
        Debug.Log("Opened arrowui");
    }
    private void StopMinigame()
    {
        CalculateArrows(false);
        minigameUI.SetActive(false);
        currentRoundPlaying = false;
        playerMovement.enabled = true;
        playerZoom.enabled = true;
        Debug.Log("closed arrow ui");

    }
    private void CalculateArrows(bool gameState)
    {
        if (gameState == true)
        {

            if (gameState == true)
            {
                float value;
                for (int i = 1; i <= _arrowAmount; i++)
                {
                    GameObject cloneArrow;
                    float min = 0f;
                    float max = _arrowAmount * 100 - 100;

                    value = i * 100 - 100; // Simulate a value in range [0, 400]

                    // Normalize and map into UI space range (-500 to +500)
                    float normalized = Mathf.InverseLerp(min, max, value);
                    float mappedX = Mathf.Lerp(-400, 400, normalized);


                    DirectionName directionEnum = (DirectionName)UnityEngine.Random.Range(0, 4);
                    arrowImage = spriteList[(int)directionEnum];



                    arrow.SetActive(true);

                    arrow.GetComponent<Image>().sprite = arrowImage;
                    cloneArrow = Instantiate(arrow);
                    // cloneArrow.transform.localScale += new Vector3(2, 2, 2);

                    cloneArrow.name = "Arrow_" + directionEnum.ToString(); // Name for debug

                    cloneArrow.transform.SetParent(Parent, false);

                    // Set UI position properly using RectTransform
                    RectTransform rt = cloneArrow.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(mappedX, 0f); // or set a Y offset if needed


                    cloneArrowList.Add(cloneArrow);
                    directionList.Add(directionEnum);

                }
            }
            currentRoundPlaying = true;
        }
        arrow.SetActive(false);
        if (gameState == false)
        {

            foreach (GameObject cloneArrow in cloneArrowList)
            {
                if (cloneArrow != null)
                {
                    Destroy(cloneArrow);
                }
            }
            cloneArrowList.Clear();
            directionList.Clear();
            currentArrowIndex = 0;

        }
    }

    private void RunRound(FactoryBase currentFactory)
    {
        factoryMultText.text = "Current speed bonus: " + currentFactory.GetComponent<FactoryBase>().speedIncreasePercentage.ToString() + "%";
        if (currentArrowIndex >= directionList.Count)
        {
            Debug.Log("All inputs correct! Round finished.");
            currentFactory.IncreaseCraftingSpeed();
            currentFactory = null;
            minigameTutorielDone = true;
            currentRoundPlaying = false;
            cloneArrowList.Clear();
            directionList.Clear();
            currentArrowIndex = 0;
            CalculateArrows(true);
            return;
        }

        DirectionName inputDirection = CheckCombo();

        if (inputDirection != DirectionName.None)
        {
            if (inputDirection == directionList[currentArrowIndex])
            {
                Debug.Log("Correct input for arrow " + currentArrowIndex);
           
                Destroy(cloneArrowList[currentArrowIndex]);
                AudioManager.Instance.PlaySFXArrayAt("ArrowMinigameSounds", currentArrowIndex);
                currentArrowIndex++;
            }
            else
            {
                Debug.Log("Incorrect input for arrow " + currentArrowIndex);
                // Optional: feedback or penalty
                currentRoundPlaying = false;
                StopMinigame();
            }
        }

    }



    private DirectionName CheckCombo()
    {
        if (UpArrow)
            return DirectionName.Up;
        if (DownArrow)
            return DirectionName.Down;
        if (LeftArrow)
            return DirectionName.Left;
        if (RightArrow)
            return DirectionName.Right;

        return DirectionName.None;
    }

}

