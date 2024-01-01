using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : MonoBehaviour, IUsable
{
    [SerializeField] private AbilityDataSO abilityDataSO;

    public void ActivateAbility()
    {
        OnShieldAbilityActivated();
    }

    public void Initialize(AbilityDataSO abilityDataSO)
    {
        this.abilityDataSO = abilityDataSO;
    }

    private void OnShieldAbilityActivated()
    {
        //TODO
        Debug.Log("Shield Ability Activated!");
    }
}
