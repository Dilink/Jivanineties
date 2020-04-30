using System;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ChangeLevelPhase : ICombatPhase
{
    public UnityEditor.SceneAsset scene;

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        yield return null;

        SceneManager.LoadScene(scene.name);

        onEnd();
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.BattlePhase;
    }
}
