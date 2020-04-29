using System;
using System.Collections;

[System.Serializable]
public class WaitForAllEnemiesDeath : ICombatPhase
{
    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        yield return null;
        while(GameManager.Instance.remainingEnemies.Count > 0)
        {
            yield return null;
        }
        onEnd();
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.BattlePhase;
    }
}
