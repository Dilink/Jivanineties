using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PhaseList.asset", menuName = "Custom/Phase List")]
public class PhaseList : ScriptableObject
{
	[InlineEditor]
	[SerializeField]
	[ShowInInspector]
	[SerializeReference]
	[ListDrawerSettings(Expanded = true)]
	public List<ICombatPhase> phases = new List<ICombatPhase>();
}
