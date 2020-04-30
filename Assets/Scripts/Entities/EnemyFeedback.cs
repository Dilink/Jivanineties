using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{
    public Animator stateAnim;

    public ParticleSystem StunFX;

    public bool isStun;
    public bool isInvicible;

    

    public void feedBackInvicibility()
    {
        if (isStun)
        {
            feedBackStunInvisibility();
        }

    }

    public void feedBackStun()
    {
        if (isInvicible)
        {
            feedBackStunInvisibility();
        }
        //animWaterShield.Play("WaterShield_Absorption");
    }

    public void feedBackStunInvisibility()
    {
        if (isInvicible)
        {

        }
        //animWaterShield.Play("WaterShield_Absorption");
    }

    public void endStun()
    {

    }

    public void endInvincibility()
    {

    }
}
