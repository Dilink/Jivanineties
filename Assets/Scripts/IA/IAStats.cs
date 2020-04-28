using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAStats" , menuName = "IAStats" , order = 0)]
public class IAStats : ScriptableObject
{
    public LifePointType[] lifePointTypes;
}

public enum LifePointType
{
    normal,
    specialAttack
}