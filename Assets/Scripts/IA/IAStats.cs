using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAStats", menuName = "IAStats", order = 0)]
public class IAStats : ScriptableObject
{
    public LifePointType[] lifePointTypes;
    public AttackStats[] Attack;
}

public enum LifePointType
{
    normal,
    specialAttack
}

[System.Serializable]
public struct AttackStats
{
    public float prepDuration;
    public float attackDuration;
    public float coolDownAfterAttack;
    public LifePointType attackTypes;
    // public GameObject hitBox;
}