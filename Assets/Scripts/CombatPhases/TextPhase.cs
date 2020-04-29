using System;
using System.Collections;

[System.Serializable]
public class TextPhase : ICombatPhase
{
    public string text;

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        GameManager.Instance.uiManager.ShowAlertText(text);
        yield return null;
    }
    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.BattlePhase;
    }
}
