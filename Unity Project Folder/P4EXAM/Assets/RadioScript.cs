
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class RadioScript : MonoBehaviour
{
    [SerializeField] int timeInSeconds;

    private bool _closeUI;

    private bool isInDialogMode =false;

    [SerializeField] private Text unreadMessagesText;
    private int amountOfUnread;
    [SerializeField] private Text personWhoMessaged;
    [SerializeField] private GameObject dialogTextGameObject;
    [SerializeField] private GameObject dialogPersonThatMessagedGameObject;
    [SerializeField] private GameObject buttonForMessages;
    private bool listenerAdded = false;

    [SerializeField] private GameObject radioUI;
    //name,Dialog
    [SerializeField] Queue<DialogList> dialogData = new Queue<DialogList>();

    //[SerializeField] List<DialogList> DialogData = new List<DialogList>();

    [System.Serializable]
   public class DialogList
    {
        public string personMessageing;
        public string dialog;
        public bool triggered = false; // tracks if it has been added
    }
    DialogList currentDialog;
private Button messageButton;
private Text dialogText;
private Text DialogPersonText;

    void Start()
{
        messageButton = buttonForMessages.GetComponent<Button>();
        dialogText = dialogTextGameObject.GetComponent<Text>();
        DialogPersonText = dialogPersonThatMessagedGameObject.GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        timeInSeconds = Mathf.FloorToInt(Time.time);

        GetKeyInfo();
        CloseUI();
        CharPicker();
        DialogController();
        if (isInDialogMode)
        {
            ShowDialog();
        }else if (!isInDialogMode){
            unreadMessagesText.enabled = true;
            dialogTextGameObject.SetActive(false);
            dialogPersonThatMessagedGameObject.SetActive(false);
            buttonForMessages.SetActive(true);

        }
    }

    private void GetKeyInfo()
    {
        _closeUI = InputHandler.Instance.PassInputBoolValue(2);
    }
        void CloseUI()
    {
        if (_closeUI == true && radioUI.activeInHierarchy)
        {
            radioUI.SetActive(false);
            isInDialogMode = false;
        }
    }
  void CharPicker()
{
    if (currentDialog == null && dialogData.Count > 0)
    {
        currentDialog = dialogData.Peek();
    }

    if (dialogData.Count > 0)
    {
        personWhoMessaged.text = currentDialog.personMessageing;
        DialogPersonText.text = currentDialog.personMessageing;

        unreadMessagesText.text = $"You have {dialogData.Count} unread messages.";
        if (!listenerAdded)
        {
            messageButton.onClick.AddListener(() => ShowDialog());
            listenerAdded = true;
        }
    }
    else
    {
        unreadMessagesText.text = "You have 0 Messages";
        messageButton.onClick.RemoveAllListeners();
        personWhoMessaged.text = "No one";
    }
}
    void DialogController()
    {
  
        if ((timeInSeconds >= 0 && timeInSeconds < 3) && (currentDialog == null || !currentDialog.triggered))
        {
            EnqueueDialog("Turiel", "Hello, are you there? Ah good, i've been told to read you a few intrustions from my notebook. Hmm let's see, if u open your blueprint book by clicking on it, you should be able to scroll though it to see diffrent buildable objects, try building a factory! Any will do.", true);
            Debug.Log("I HAVE QUEUED");
        }
        if ((timeInSeconds >= 10 && timeInSeconds < 12) && (currentDialog == null || !currentDialog.triggered))
        {
            EnqueueDialog("Turiel", "Next up try using your drones, you should be able to move them between" + FlagManager.Instance._allowedFlagCount + "diffrent locations, by making them pass over building like factorys, they should be able to take the output and move them around into another building to craft items. For example transfering copper into a wire factory, it shoooould make wires for you to make spools and uhhh you know stuff with.", true);
            Debug.Log("I HAVE QUEUED");
 
        }
    }


    void EnqueueDialog(string person, string dialog, bool hasTriggered)
    {
        // Create a new dialog and enqueue it
        DialogList newDialog = new DialogList
        {
            personMessageing = person,
            dialog = dialog,
            triggered = hasTriggered
        };

        dialogData.Enqueue(newDialog);
    }

    void ShowDialog()
    {
        if (!isInDialogMode)
        {
            // Display the dialog (e.g., show it in the UI)
            dialogText.text = currentDialog.dialog;

            // Once the dialog is shown, dequeue it
            dialogData.Dequeue();
            currentDialog = null;
            isInDialogMode = true;

        }
        else
        {
            unreadMessagesText.enabled = false;
            dialogTextGameObject.SetActive(true);
            dialogPersonThatMessagedGameObject.SetActive(true);
            buttonForMessages.SetActive(false);


        }
    }
}
