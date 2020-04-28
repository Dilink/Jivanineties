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
			Debug.Log("Waiting " + remaining + "s");
			remaining -= 1.0f;
			yield return new WaitForSeconds(1.0f);
		}

		if (remaining > 0.0f)
		{
			Debug.Log("Waiting " + remaining + "s");
			yield return new WaitForSeconds(remaining);
		}

		Debug.Log("Waiting finished");
		onEnd();
	}
}
