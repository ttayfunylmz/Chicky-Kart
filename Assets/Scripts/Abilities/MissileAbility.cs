using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAbility : MonoBehaviour, IUsable
{
    [SerializeField] private AbilityDataSO abilityDataSO;

    public void ActivateAbility()
    {
        OnMissileAbilityActivated();
    }

    public void Initialize(AbilityDataSO abilityDataSO)
    {
        this.abilityDataSO = abilityDataSO;
    }

    private void OnMissileAbilityActivated()
    {
        //TODO
        Debug.Log("Missile Ability Activated!");
    }
}
