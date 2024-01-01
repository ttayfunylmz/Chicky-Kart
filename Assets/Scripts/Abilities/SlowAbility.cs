using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAbility : MonoBehaviour, IUsable
{
    [SerializeField] private AbilityDataSO abilityDataSO;

    public void ActivateAbility()
    {
        OnSlowAbilityActivated();
    }

    public void Initialize(AbilityDataSO abilityDataSO)
    {
        this.abilityDataSO = abilityDataSO;
    }

    private void OnSlowAbilityActivated()
    {
        //TODO
        Debug.Log("Slow Ability Activated!");
    }
}
