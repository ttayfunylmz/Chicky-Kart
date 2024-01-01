using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MysteryBox : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] private float respawnDuration = 5f;

    public void DisableMysteryBox()
    {
        DisableMysteryBoxServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisableMysteryBoxServerRpc()
    {
        DisableMysteryBoxClientRpc();
    }

    [ClientRpc]
    private void DisableMysteryBoxClientRpc()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator RespawnMysteryBoxAfterDelay()
    {
        yield return new WaitForSeconds(respawnDuration);
        RespawnMysteryBoxAfterDelayServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RespawnMysteryBoxAfterDelayServerRpc()
    {
        RespawnMysteryBoxAfterDelayClientRpc();
    }

    [ClientRpc]
    private void RespawnMysteryBoxAfterDelayClientRpc()
    {
        gameObject.SetActive(true);
    }
}
