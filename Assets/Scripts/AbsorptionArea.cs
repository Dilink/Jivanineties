using UnityEngine;

public class AbsorptionArea : MonoBehaviour, IAbsorbable
{
    public AbsorptionMaterialChange materials;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materials.materialBefore;
    }

    public bool OnAbsorption()
    {
        if (meshRenderer.material == materials.materialAfter)
        {
            return false;
        }

        meshRenderer.material = materials.materialAfter;
        return true;
    }

    public bool OnRestore()
    {
        if (meshRenderer.material == materials.materialBefore)
        {
            return false;
        }

        meshRenderer.material = materials.materialBefore;
        return true;
    }
}
