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
            AbsorptionArea.onAbsorptionDelegate += OnAbsorption;
        }

        else if (actionType == AbsorptionType.RESTORE)
        {
            AbsorptionArea.onRestoreDelegate += OnRestore;
        }
    }

    private void OnAbsorption(AbsorptionArea p_area)
    {
        if (area == null)
        {
            AbsorptionArea.onAbsorptionDelegate -= OnAbsorption;
            onPhaseEnd();
        }
        else if (area != null && area == p_area)
        {
            AbsorptionArea.onAbsorptionDelegate -= OnAbsorption;
            onPhaseEnd();
        }
    }

    private void OnRestore(AbsorptionArea p_area)
    {
        if (area == null)
        {
            AbsorptionArea.onRestoreDelegate -= OnRestore;
            onPhaseEnd();
        }
        else if (area != null && area == p_area)
        {
            AbsorptionArea.onRestoreDelegate -= OnRestore;
            onPhaseEnd();
        }
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
