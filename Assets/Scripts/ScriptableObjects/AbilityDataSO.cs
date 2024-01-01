using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilities", menuName = "Abilities/Create New Ability")]
public class AbilityDataSO : ScriptableObject
{
    public AbilityManager.AbilityType abilityType;
    public string skillName;
    // public Sprite icon;
}
