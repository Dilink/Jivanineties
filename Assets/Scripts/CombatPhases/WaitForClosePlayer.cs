using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class WaitForClosePlayer : ICombatPhase
{
    public float distance = 1.0f;
    public Vector3 center = Vector3.zero;

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        PlayerController player = GameManager.Instance.player;

        yield return null;
        
        while (Vector3.Distance(player.transform.position, center) > distance)
        {
            yield return null;
        }

        onEnd();
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
