using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static BlueprintBook;

public class RadioScript : MonoBehaviour
{

    private bool _closeUI;
    private Text unreadMessagesText;
    private int amountOfUnread;
    private Text PersonWhoMessaged;
    private Button ButtonForMessages;
    private bool listenerAdded = false;

    [SerializeField] private GameObject radioUI;
    //name,Dialog

    [SerializeField] List<DialogList> = new List<DialogList>();

            [System.Serializable]
   public class DialogList
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetKeyInfo();
        CloseUI();
        CharPicker();
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
        }
    }
    void CharPicker()
    {
        if (amountOfUnread <= 1)
        {
            if (listenerAdded == false) {
                ButtonForMessages.onClick.AddListener(() => DialogController());
                listenerAdded = true;
            }
        }
    }
    void DialogController()
    {

    }
}
