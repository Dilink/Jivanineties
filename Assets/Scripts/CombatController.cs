using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatController : MonoBehaviour
{
	[InlineEditor]
	[SerializeField]
	[ShowInInspector]
	[SerializeReference]
	[ListDrawerSettings(Expanded = true)]
	public List<ICombatPhase> phases = new List<ICombatPhase>();

	[InlineEditor]
	[SerializeField]
	[ShowInInspector]
	[SerializeReference]
	[ReadOnly]
	public ICombatPhase currentPhase;

	private void OnPhaseEnd()
	{
		MoveToNextPhase();
	}

	[Button]
	private void TestRunPhases()
	{
		MoveToNextPhase();
	}

	public bool MoveToNextPhase()
	{
		if (phases.Count == 0)
		{
			currentPhase = null;
			return false;
		}

		ICombatPhase phase = phases[0];
		phases.RemoveAt(0);
		currentPhase = phase;

		CombatPhaseData data;
		data.Instantiate = Instantiate;

		StartCoroutine(phase.Execute(OnPhaseEnd, data));
		return true;
	}
}
