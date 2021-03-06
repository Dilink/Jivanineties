﻿using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Custom/Attack", order = 1)]
public class Attack : ScriptableObject
{

    public int damage;
    public float hitBoxDuration;
    public float attackRecoveryDuration;
    public float attackCoolDownDuration;
    public float preparationDuration;
    public float dodgeWindowDuration;
    public float stunDuration;
    public Vector3 rangeBox;
    public Vector3 offset;

    public LifePointType attackType;

}
