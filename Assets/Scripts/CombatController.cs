using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class BattlePhase : ICombatPhase
{
	private readonly SpawnPoint[] spawnPoints;
	private readonly Func<UnityEngine.Object, Vector3, Quaternion, UnityEngine.Object> Instantiate;
	
	private static readonly System.Random random = new System.Random();

	public BattlePhase(Func<UnityEngine.Object, Vector3, Quaternion, UnityEngine.Object> Instantiate, params SpawnPoint[] spawnPoints)
	{
		this.Instantiate = Instantiate;
		this.spawnPoints = spawnPoints;
	}

	private Vector3 FindRandomLocationNearCenter(Vector3 center, float radius)
	{
		float angle = (float)(2.0 * Math.PI * random.NextDouble());
		
		float randX = center.x + radius * Mathf.Cos(angle);
		float randZ = center.z + radius * Mathf.Sin(angle);
		return new Vector3(randX, center.y, randZ);
	}

	public IEnumerator Execute(Action onEnd)
	{
		foreach (var sp in spawnPoints)
		{
			var pos = FindRandomLocationNearCenter(sp.location, sp.radius);
			Instantiate(sp.prefab, pos, Quaternion.identity);
		}

		onEnd();
		yield return null;
	}
}

public class WaitPhase : ICombatPhase
{
	private readonly float time;

	public WaitPhase(float time)
	{
		this.time = time;
	}

	public IEnumerator Execute(Action onEnd)
	{
		yield return new WaitForSeconds(time);
		onEnd();
	}
}

public class CombatController : MonoBehaviour
{
	[InlineEditor]
	public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

	private Queue<ICombatPhase> phases = new Queue<ICombatPhase>();
	
	void Start()
	{
		phases.Enqueue(new WaitPhase(1.0f));
		phases.Enqueue(new BattlePhase(Instantiate, spawnPoints[0]));
		phases.Enqueue(new WaitPhase(3.0f));
		phases.Enqueue(new BattlePhase(Instantiate, spawnPoints[0], spawnPoints[0]));

		MoveToNextPhase();
	}

	private void OnPhaseEnd()
	{
		MoveToNextPhase();
	}

	public void MoveToNextPhase()
	{
		if (phases.Count == 0)
		{
			return;
		}

		ICombatPhase phase = phases.Dequeue();
		StartCoroutine(phase.Execute(OnPhaseEnd));
	}
}
