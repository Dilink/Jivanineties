using UnityEngine;

[CreateAssetMenu(fileName = "AbsorptionMaterialChange.asset", menuName = "Custom/Absorption Material Change")]
public class AbsorptionMaterialChange : ScriptableObject
{
    [SerializeField]
    public Material materialBefore;

    [SerializeField]
    public Material materialAfter;
}
