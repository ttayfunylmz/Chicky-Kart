using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAbility : MonoBehaviour, IUsable
{
    [SerializeField] private AbilityDataSO abilityDataSO;

    public void ActivateAbility()
    {
        OnSpeedAbilityActivated();
    }

    public void Initialize(AbilityDataSO abilityDataSO)
    {
        this.abilityDataSO = abilityDataSO;
    }

    private void OnSpeedAbilityActivated()
    {
        //TODO
        Debug.Log("Speed Ability Activated!");
    }
}
