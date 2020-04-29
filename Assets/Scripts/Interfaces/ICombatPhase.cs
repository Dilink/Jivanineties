using System;
using System.Collections;
using UnityEngine;

public struct CombatPhaseData
{
    public Func<UnityEngine.Object, Vector3, Quaternion, UnityEngine.Object> Instantiate;
}

public interface ICombatPhase
{
    IEnumerator Execute(Action onEnd, CombatPhaseData data);
}
