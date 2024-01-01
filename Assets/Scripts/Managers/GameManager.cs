using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnStateChanged;
    public event Action OnLocalPlayerReadyChanged;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3f);
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake() 
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            OnLocalPlayerReady();
        }

        if (!IsServer) { return; }

        switch(state.Value)
        {
            case State.WaitingToStart:
                break;

            case State.CountdownToStart:
                countDownToStartTimer.Value -= Time.deltaTime;
                if(countDownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                }
                break;

            case State.GamePlaying:
                //WHEN THE GAME IS OVER, COME HERE
                break;

            case State.GameOver:
                break;
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke();
    }

    private void OnLocalPlayerReady()
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke();
            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool isAllClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDictionary.ContainsKey(clientId) ||!playerReadyDictionary[clientId])
            {
                //PLAYER IS NOT READY
                isAllClientsReady = false;
                break;
            }
        }

        if(isAllClientsReady)
        {
            state.Value = State.CountdownToStart;
        }
    }

    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }

    public bool IsGamePlayingActive()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
}
