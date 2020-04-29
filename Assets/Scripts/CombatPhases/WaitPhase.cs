using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class WaitPhase : ICombatPhase
{
	public float time;

	public IEnumerator Execute(Action onEnd, CombatPhaseData data)
	{
		float remaining = time;

		while (remaining > 1.0f)
		{
			Debug.Log(GameManager.Instance);
			Debug.Log(GameManager.Instance.uiManager);
			GameManager.Instance.uiManager.ShowAlertText("New combat in " + remaining + "s");
			remaining -= 1.0f;
			yield return new WaitForSeconds(1.0f);
		}

		if (remaining > 0.0f)
		{
			GameManager.Instance.uiManager.ShowAlertText("New combat in " + remaining + "s");
			yield return new WaitForSeconds(remaining);
		}

		onEnd();
	}

	public CombatPhaseType ReturnType()
	{
		return CombatPhaseType.WaitPhase;
	}
}
