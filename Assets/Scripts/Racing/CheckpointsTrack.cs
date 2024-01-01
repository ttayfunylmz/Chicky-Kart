using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CheckpointsTrack : NetworkBehaviour
{
    public static CheckpointsTrack Instance { get; private set; }

    private const int MAX_LAPS = 4;

    public event Action OnPlayerCorrectCheckpoint;
    public event Action OnPlayerWrongCheckpoint;
    public event Action<int> OnPlayerLapCompleted;
    public event Action OnPlayerRaceCompleted;

    [SerializeField] private List<Checkpoint> checkpoints;
    [SerializeField] private List<NetworkObject> kartNetworkObjectsList;
    [SerializeField] private Transform checkpointsTransform;

    private Dictionary<NetworkObject, int> nextCheckpointIndexDictionary;

    private int lapCounter;

    private void Awake() 
    {
        Instance = this;

        checkpoints = new List<Checkpoint>();

        foreach(Transform checkpointTransform in checkpointsTransform)
        {
            Checkpoint checkpoint = checkpointTransform.GetComponent<Checkpoint>();
            checkpoint.SetCheckpointsTrack(this);
            checkpoints.Add(checkpoint);
        }

        nextCheckpointIndexDictionary = new Dictionary<NetworkObject, int>();

        lapCounter = 0;
    }

    public void AddKart(NetworkObject kartNetworkObject)
    {
        kartNetworkObjectsList.Add(kartNetworkObject);
        nextCheckpointIndexDictionary[kartNetworkObject] = 0;
    }

    public void RemoveKart(NetworkObject kartNetworkObject)
    {
        int index = kartNetworkObjectsList.IndexOf(kartNetworkObject);
        if (index != -1)
        {
            kartNetworkObjectsList.RemoveAt(index);
            nextCheckpointIndexDictionary.Remove(kartNetworkObject);
        }
    }

    public void KartThroughCheckpoint(Checkpoint checkpoint, NetworkObject kartNetworkObject)
    {
        if (nextCheckpointIndexDictionary.ContainsKey(kartNetworkObject))
        {
            int nextCheckpointIndex = nextCheckpointIndexDictionary[kartNetworkObject];

            if (checkpoints.IndexOf(checkpoint) == nextCheckpointIndex)
            {
                if (kartNetworkObject.IsOwner)
                {
                    UpdatePlayerPositions();
                    IsLapCompleted(nextCheckpointIndex);
                    OnPlayerCorrectCheckpoint?.Invoke();
                }

                nextCheckpointIndexDictionary[kartNetworkObject] = (nextCheckpointIndex + 1) % checkpoints.Count;
            }
            else
            {
                if(kartNetworkObject.IsOwner)
                {
                    OnPlayerWrongCheckpoint?.Invoke();
                }
                
            }
        }
    }

    private void IsLapCompleted(int nextCheckpointIndex)
    {
        if(nextCheckpointIndex == 0)
        {
            lapCounter++;
            OnPlayerLapCompleted?.Invoke(lapCounter);
            IsRaceCompleted(nextCheckpointIndex);
        }
    }

    private void IsRaceCompleted(int nextCheckpointIndex)
    {
        if(lapCounter == MAX_LAPS)
        {
            OnPlayerRaceCompleted?.Invoke();
        }
    }

    public int GetNextCheckpointIndex(NetworkObject player)
    {
        if (nextCheckpointIndexDictionary.TryGetValue(player, out int index))
        {
            return index;
        }

        return -1;
    }

    public void UpdatePlayerPositions()
    {
        Dictionary<NetworkObject, float> distancesToCheckpoints = new Dictionary<NetworkObject, float>();

        foreach (NetworkObject player in kartNetworkObjectsList)
        {
            if (player.IsOwner)
            {
                int nextCheckpointIndex = GetNextCheckpointIndex(player);
                if (nextCheckpointIndex != -1 && nextCheckpointIndex < checkpoints.Count)
                {
                    Checkpoint nextCheckpoint = checkpoints[nextCheckpointIndex];
                    float distance = CalculateDistanceToCheckpoint(player, nextCheckpoint);

                    // Adjusting to consider ties in distance
                    while (distancesToCheckpoints.ContainsKey(player) && distancesToCheckpoints[player] == distance)
                    {
                        distance += 0.0001f; // Offset for ties to avoid same distance conflict
                    }
                    distancesToCheckpoints.Add(player, distance);
                }
                else
                {
                    Debug.LogWarning("Invalid next checkpoint index for player: " + player.NetworkObjectId);
                }
            }
        }

        var sortedPlayers = distancesToCheckpoints.OrderBy(x => x.Value).ToList();

        int position = 1; // Initial position

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            NetworkObject player = sortedPlayers[i].Key;

            // Handling equal positions for players with equal distances
            if (i > 0 && sortedPlayers[i].Value > sortedPlayers[i - 1].Value)
            {
                position = i + 1; // Increment position only when distances change
            }

            Debug.Log("Player " + player.OwnerClientId + " is at position: " + position);
        }
    }

    private float CalculateDistanceToCheckpoint(NetworkObject player, Checkpoint checkpoint)
    {
        Vector3 playerPosition = player.GetComponent<Transform>().position;
        Vector3 checkpointPosition = checkpoint.transform.position;

        return Vector3.Distance(playerPosition, checkpointPosition);
    }
}
