﻿using UnityEngine;

public class AbsorptionArea : MonoBehaviour, IAbsorbable
{
    public AbsorptionMaterialChange materials;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materials.materialBefore;
    }

    public void OnAbsorption()
    {
        meshRenderer.material = materials.materialAfter;
    }
}