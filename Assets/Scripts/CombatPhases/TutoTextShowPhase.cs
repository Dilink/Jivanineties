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
        GameManager.Instance.uiManager.ShowTutoText(text);

        onEnd();

        yield return null;
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
