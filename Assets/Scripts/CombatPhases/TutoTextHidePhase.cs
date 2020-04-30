using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class TutoTextHidePhase : ICombatPhase
{
    public int fix;

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        GameManager.Instance.uiManager.HideTutoText();

        onEnd();

        yield return null;
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
