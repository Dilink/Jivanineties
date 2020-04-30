using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class TutoTextShowPhase : ICombatPhase
{
    [TextArea(1, 5)]
    public string text;

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        yield return null;

        GameManager.Instance.uiManager.ShowTutoText(text);

        onEnd();

    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
