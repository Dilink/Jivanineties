﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class BattlePhase : ICombatPhase
{
	[InlineEditor]
	[SerializeField]
	[ListDrawerSettings(Expanded = true)]
	public List<Spawner> spawners = new List<Spawner>();

	private static readonly System.Random random = new System.Random();

	private Vector3 FindRandomLocationNearCenter(Vector3 center, float radius)
	{
		float angle = (float)(2.0 * Math.PI * random.NextDouble());

		float randX = center.x + radius * Mathf.Cos(angle);
		float randZ = center.z + radius * Mathf.Sin(angle);

		Vector3 pos = new Vector3(randX, center.y, randZ);

		NavMeshHit hit;
		NavMesh.SamplePosition(pos, out hit, radius / 2.0f, NavMesh.GetAreaFromName("Walkable"));

		if (hit.position.x != Mathf.Infinity)
		{
			return hit.position;
		}

		return pos;
	}

	public IEnumerator Execute(Action onEnd, CombatPhaseData data)
	{
		GameManager.Instance.uiManager.ShowAlertText("Enemy spawned!");

		foreach (var s in spawners)
		{
			for (int i = 0; i < s.creationCount; i++)
			{
				var pos = FindRandomLocationNearCenter(s.spawnPoint.location, s.spawnPoint.radius);
				data.Instantiate(s.prefab, pos, Quaternion.identity);
			}
		}

		onEnd();
		yield return null;
	}
	public CombatPhaseType ReturnType()
	{
		return CombatPhaseType.BattlePhase;
	}
}
