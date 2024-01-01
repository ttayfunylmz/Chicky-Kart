using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Checkpoint : NetworkBehaviour
{
    private CheckpointsTrack checkpointsTrack;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<PlayerNetwork>(out PlayerNetwork player))
        {
            NetworkObject networkObject = player.GetComponent<NetworkObject>();
            if(networkObject != null)
            {
                checkpointsTrack.KartThroughCheckpoint(this, networkObject);
            }
        }
    }

    public void SetCheckpointsTrack(CheckpointsTrack checkpointsTrack)
    {
        this.checkpointsTrack = checkpointsTrack;
    }
}
