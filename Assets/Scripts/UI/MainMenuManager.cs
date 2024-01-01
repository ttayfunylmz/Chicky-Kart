using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TMP_InputField joinCodeInputField;

    private void Awake() 
    {
        hostButton.onClick.AddListener(() =>
        {
            StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            StartClient();
        });
    }

    public async void StartHost()
    {
        await HostSingleton.Instance.hostGameManager.StartHostAsync();
    }

    public async void StartClient()
    {
        await ClientSingleton.Instance.clientGameManager.StartClientAsync(joinCodeInputField.text);
    }
    
}
