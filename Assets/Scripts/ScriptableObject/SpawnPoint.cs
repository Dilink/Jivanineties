using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPoint.asset", menuName = "Custom/SpawnPoint")]
public class SpawnPoint : ScriptableObject
{
    [SerializeField]
    public Vector3 location;

    [SerializeField]
    public float radius = 1.0f;

    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public int creationCount = 1;
}
