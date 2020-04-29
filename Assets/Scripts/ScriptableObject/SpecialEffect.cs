using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialEffect", menuName = "Custom/SpecialEffect", order = 1)]
public class SpecialEffect : ScriptableObject
{
    public float effectDuration;

    [Header("Camera Rotation")]
    [Curve(0, -50f, 10f, 100f, true)]
    public AnimationCurve camRotationX;
    [Curve(0, -50f, 10f, 100f, true)]
    public AnimationCurve camRotationY;
    [Curve(0, -50f, 10f, 100f, true)]
    public AnimationCurve camRotationZ;

    [Header("Camera Position")]
    [Curve(0, -10f, 10f, 20f, true)]
    public AnimationCurve camPositionX;
    [Curve(0, -10f, 10f, 20f, true)]
    public AnimationCurve camPositionY;
    [Curve(0, -10f, 10f, 20f, true)]
    public AnimationCurve camPositionZ;

    [Header("Special")]
    [Curve(0, -50f, 10f, 100f, true)]
    public AnimationCurve camZoom;
    [Curve(0, 0.1f, 10f, 1.9f, true)]
    public AnimationCurve timeScale;
    public SpecialEffect nextEffect;

}
