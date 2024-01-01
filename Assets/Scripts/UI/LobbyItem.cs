using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private TMP_Text lobbyPlayersText;

    private LobbiesList lobbiesList;
    private Lobby lobby;

    private void Awake() 
    {
        joinButton.onClick.AddListener(() =>
        {
            Join();
        });
    }

    public void Initialize(LobbiesList lobbiesList, Lobby lobby)
    {
        this.lobbiesList = lobbiesList;
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        lobbyPlayersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
    }

    private void Join()
    {
        lobbiesList.JoinAsync(lobby);
    }
}
