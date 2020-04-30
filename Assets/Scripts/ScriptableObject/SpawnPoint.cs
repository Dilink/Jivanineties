using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPoint.asset", menuName = "Custom/SpawnPoint")]
public class SpawnPoint : ScriptableObject
{
    [SerializeField]
    public Vector3 location;

    [SerializeField]
    public float radius = 1.0f;
}
