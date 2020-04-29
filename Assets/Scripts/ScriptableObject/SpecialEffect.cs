using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialEffect", menuName = "Custom/SpecialEffect", order = 1)]
public class SpecialEffect : ScriptableObject
{
    public float effectDuration;
    public bool followPlayer;

    [Header("Camera Rotation")]
    [Curve(0, -50f, 2f, 100f, true)]
    public AnimationCurve camRotationX;
    [Curve(0, -50f, 2f, 100f, true)]
    public AnimationCurve camRotationY;
    [Curve(0, -50f, 2f, 100f, true)]
    public AnimationCurve camRotationZ;

    [Header("Camera Position")]
    [Curve(0, -2.5f, 2f, 5f, true)]
    public AnimationCurve camPositionX;
    [Curve(0, -2.5f, 2f, 5f, true)]
    public AnimationCurve camPositionY;
    [Curve(0, -2.5f, 2f, 5f, true)]
    public AnimationCurve camPositionZ;

    [Header("Special")]
    [Curve(0, -25f, 2f, 50f, true)]
    public AnimationCurve camZoom;
    [Curve(0, 0.25f, 2f, 1.5f, true)]
    public AnimationCurve timeScale;
}
