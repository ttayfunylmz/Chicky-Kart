using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
    public const string PLAYER_NAME = "PlayerName";

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button connectButton;
    [SerializeField] private int minNameLength = 1;
    [SerializeField] private int maxNameLength = 12;

    private void Awake() 
    {
        connectButton.onClick.AddListener(() =>
        {
            Connect();
        });
    }

    private void Start()
    {
        if(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        nameInputField.text = PlayerPrefs.GetString(PLAYER_NAME, string.Empty);
        HandleNameChanged();
    }

    public void HandleNameChanged()
    {
        connectButton.interactable =
            nameInputField.text.Length >= minNameLength &&
            nameInputField.text.Length <= maxNameLength;

    }

    private void Connect()
    {
        PlayerPrefs.SetString(PLAYER_NAME, nameInputField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
