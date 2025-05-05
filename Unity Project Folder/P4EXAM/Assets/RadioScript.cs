
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
    [SerializeField] private Text dialogText;
    [SerializeField] private Button buttonForMessages;
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
        // If there are dialog messages to process

        if (dialogData.Count > 0)
        {
            personWhoMessaged.text = currentDialog.personMessageing;

            unreadMessagesText.text = $"You have {dialogData.Count} unread messages.";
            if (!listenerAdded)
            {
                buttonForMessages.onClick.AddListener(() => ShowDialog());
                listenerAdded = true;
            }
        }
        else if (dialogData.Count == 0)
        {

            unreadMessagesText.text = "You have 0 Messages";
            personWhoMessaged.text = "No one";
        }
    }
    void DialogController()
    {
  
        if ((timeInSeconds >= 0 && timeInSeconds < 3) && (currentDialog == null || !currentDialog.triggered))
        {
            EnqueueDialog("Radio Operator", "Hello, are you still there?", true);
            Debug.Log("I HAVE QUEUED");

            currentDialog = dialogData.Peek();
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
            isInDialogMode = true;

        }
        else
        {

        }
    }
}
