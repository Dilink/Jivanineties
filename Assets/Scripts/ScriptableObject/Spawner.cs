using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Spawner.asset", menuName = "Custom/Spawner")]
public class Spawner : ScriptableObject
{
	[InlineEditor]
	[SerializeField]
	public SpawnPoint spawnPoint;

	[SerializeField]
	public GameObject prefab;

	[SerializeField]
	public int creationCount = 1;
}
