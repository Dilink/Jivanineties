using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class BattlePhase : ICombatPhase
{
	[InlineEditor]
	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

	private static readonly System.Random random = new System.Random();

	private Vector3 FindRandomLocationNearCenter(Vector3 center, float radius)
	{
		float angle = (float)(2.0 * Math.PI * random.NextDouble());

		float randX = center.x + radius * Mathf.Cos(angle);
		float randZ = center.z + radius * Mathf.Sin(angle);
		return new Vector3(randX, center.y, randZ);
	}

	public IEnumerator Execute(Action onEnd, CombatPhaseData data)
	{
		foreach (var sp in spawnPoints)
		{
			for (int i = 0; i < sp.creationCount; i++)
			{
				var pos = FindRandomLocationNearCenter(sp.location, sp.radius);
				data.Instantiate(sp.prefab, pos, Quaternion.identity);
			}
		}

		onEnd();
		yield return null;
	}
}
