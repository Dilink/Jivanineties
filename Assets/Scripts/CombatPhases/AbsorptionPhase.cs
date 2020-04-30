using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public enum AbsorptionType
{
    ABSORB,
    RESTORE,
}

[System.Serializable]
public class AbsorptionPhase : ICombatPhase
{
    public AbsorptionType actionType;
    public AbsorptionArea area;

    private Action onPhaseEnd;

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        yield return null;

        onPhaseEnd = onEnd;

        if (actionType == AbsorptionType.ABSORB)
        {
            area.onAbsorptionDelegate += OnAbsorption;
        }

        else if (actionType == AbsorptionType.RESTORE)
        {
            area.onRestoreDelegate += OnRestore;
        }
    }

    private void OnAbsorption()
    {
        area.onAbsorptionDelegate -= OnAbsorption;
        onPhaseEnd();
    }

    private void OnRestore()
    {
        area.onRestoreDelegate -= OnRestore;
        onPhaseEnd();
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
