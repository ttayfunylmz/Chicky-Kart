using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private const string MYSTERY_BOX = "MysteryBox";

    private NetworkObject networkObject;

    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private int ownerPriority = 15;

    private AbilityManager abilityManager;

    public override void OnNetworkSpawn()
    {
        networkObject = GetComponent<NetworkObject>();
        CheckpointsTrack.Instance.AddKart(networkObject);

        if(IsServer)
        {
            UserData userData = HostSingleton.Instance.hostGameManager.networkServer.GetUserDataByClientId(OwnerClientId);
            playerName.Value = userData.userName;
        }

        if(IsOwner)
        {
            virtualCamera.Priority = ownerPriority;
        }
    }

    private void Awake()
    {
        abilityManager = GetComponent<AbilityManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MYSTERY_BOX))
        {
            if (!IsOwner) { return; }
            MysteryBox mysteryBox = other.GetComponent<MysteryBox>();

            if (mysteryBox != null)
            {
                Debug.Log("Pickup Collected");
                mysteryBox.DisableMysteryBox();

                AbilityDataSO selectedSkill = abilityManager.InitializeRandomAbility();

                StartCoroutine(SkillsUI.Instance.SpinWheel(selectedSkill));

                StartCoroutine(mysteryBox.RespawnMysteryBoxAfterDelay());
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            abilityManager.ActivateActiveAbility();
            SkillsUI.Instance.ResetUI();
        }
    }
}
