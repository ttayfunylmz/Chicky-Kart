using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAbility : MonoBehaviour, IUsable
{
    [SerializeField] private AbilityDataSO abilityDataSO;

    public void ActivateAbility()
    {
        OnStunAbilityActivated();
    }

    public void Initialize(AbilityDataSO abilityDataSO)
    {
        this.abilityDataSO = abilityDataSO;
    }

    private void OnStunAbilityActivated()
    {
        //TODO
        Debug.Log("Stun Ability Activated!");
    }
}
