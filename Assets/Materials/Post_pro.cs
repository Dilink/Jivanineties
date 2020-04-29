using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Post_pro : MonoBehaviour
{

    // Post Process Material
    public Material PostProcessMat;

    private void Awake()
    {
        if (PostProcessMat == null)
        {
            // Disable script if not material is assigned
            enabled = false;
        }
        else
        {
            // This is to prevent a know bug in some Unity3D versions
            PostProcessMat.mainTexture = PostProcessMat.mainTexture;
        }
    }

    // Function called by Unity after all rendering is complete to render image
    // Here we copy the render source image (source) to the final render image (destination) applying out mat (PostProcessMat)
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, PostProcessMat);
    }

}