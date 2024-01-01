using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public enum AbilityType
    {
        Missile,
        Slow,
        Shield,
        Speed,
        Stun,
    }

    [SerializeField] private List<AbilityDataSO> skillCollection = new List<AbilityDataSO>();
    [SerializeField] private AbilityDataSO activeSkillData;

    private IUsable activeAbility;

    public AbilityDataSO InitializeRandomAbility()
    {
        activeSkillData = GetRandomSkill();
        activeAbility = CreateUsableAbility(activeSkillData);

        return activeSkillData;
    }

    public void ActivateActiveAbility()
    {
        if (activeAbility != null && !SkillsUI.Instance.isSpinning)
        {
            activeAbility.ActivateAbility();
            activeAbility = null;
        }
        else
        {
            Debug.LogWarning("There is no ability to use!");
        }
    }

    private AbilityDataSO GetRandomSkill()
    {
        if (skillCollection.Count == 0)
        {
            Debug.LogError("No SkillDataSOs found in the collection.");
            return null;
        }

        int randomIndex = Random.Range(0, skillCollection.Count);
        AbilityDataSO randomSkillData = skillCollection[randomIndex];

        Debug.Log("Random Skill: " + randomSkillData.skillName);
        return randomSkillData;
    }

    private IUsable CreateUsableAbility(AbilityDataSO abilityData)
    {
        GameObject abilityObject = new GameObject("Ability_" + abilityData.skillName);
        IUsable ability = null;

        switch (abilityData.abilityType)
        {
            case AbilityType.Missile:
                abilityObject.AddComponent<MissileAbility>();
                break;
            case AbilityType.Slow:
                abilityObject.AddComponent<SlowAbility>();
                break;
            case AbilityType.Shield:
                abilityObject.AddComponent<ShieldAbility>();
                break;
            case AbilityType.Speed:
                abilityObject.AddComponent<SpeedAbility>();
                break;
            case AbilityType.Stun:
                abilityObject.AddComponent<StunAbility>();
                break;
            default:
                Debug.LogError("Unknown ability type: " + abilityData.abilityType);
                break;
        }

        ability = abilityObject.GetComponent<IUsable>();

        ability.Initialize(abilityData);

        return ability;
    }
}
