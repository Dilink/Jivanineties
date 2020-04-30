﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    public ParticleSystem[] playerHit;
    public ParticleSystem[] playerHit_Up;
    public ParticleSystem[] EnemyHit;

    public ParticleSystem[] dash;
    public ParticleSystem[] dash_Up;

    public ParticleSystem[] Recover;

    public Animator animWaterShield;
    public Animator animWaterAbsorption;


    public bool SpecialAttack { get; set; }
    public bool SpecialDash { get; set; }
    public bool AttackTouch { get; set; }
    public bool inRecover { get; set; }

    public void attackFeedback()
    {
        if (AttackTouch)
        {
            if (SpecialAttack)
            {
                for (int i = 0; i < playerHit_Up.Length; i++)
                {

                    playerHit_Up[i].Play();
                }

            }
            else
            {
                for (int i = 0; i < playerHit.Length; i++)
                {

                    playerHit[i].Play();
                }
            }
        }
    }

    public void dashFeedback()
    {
        if (SpecialDash)
        {
            for (int i = 0; i < dash_Up.Length; i++)
            {

                dash_Up[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < dash.Length; i++)
            {

                dash[i].Play();
            }
        }
    }

    public void FeedBackTakeDamage()
    {
        for (int i = 0; i < EnemyHit.Length; i++)
        {

            EnemyHit[i].Play();
        }

    }

    public void RecoverFeedBack()
    {
        if (inRecover)
        {
            for (int i = 0; i < Recover.Length; i++)
            {
                Recover[i].Play();
            }
        }
        inRecover = false;

    }

    public void PlayWaterShield()
    {
        animWaterShield.Play("WaterShield_Absorption");
    }

    public void PlayWaterAbsorption()
    {
        animWaterAbsorption.Play("WaterShield_Absorption");
    }
}
