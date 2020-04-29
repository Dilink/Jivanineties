using System;
using System.Collections;
using UnityEngine;

public enum CombatPhaseType
{
    BattlePhase,
    WaitPhase
}

public struct CombatPhaseData
{
    public Func<UnityEngine.Object, Vector3, Quaternion, UnityEngine.Object> Instantiate;
}

public interface ICombatPhase
{
    CombatPhaseType ReturnType();
    IEnumerator Execute(Action onEnd, CombatPhaseData data);
}
