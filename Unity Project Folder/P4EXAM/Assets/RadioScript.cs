using UnityEngine;

public class RadioScript : MonoBehaviour
{

    private bool _closeUI;
    [SerializeField] private GameObject radioUI;

    // Update is called once per frame
    void Update()
    {
        CloseUI();
    }

    private void GetKeyInfo()
    {
        _closeUI = InputHandler.Instance.PassInputBoolValue(2);
    }
        void CloseUI()
    {
        if (_closeUI ==true && radioUI.activeInHierarchy)
        {

        }
    }
}
