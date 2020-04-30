using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class TokendoPhase : ICombatPhase
{
	public int amount;

	public IEnumerator Execute(Action onEnd, CombatPhaseData data)
	{
		GameManager.Instance.tokendoAmount += amount;

		onEnd();

		yield return null;
	}

	public CombatPhaseType ReturnType()
	{
		return CombatPhaseType.WaitPhase;
	}
}
