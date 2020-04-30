using System.Collections;
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

    //public Ma_SoundManager sM;


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
                GameManager.Instance.sM.PlaySoundPositioned(GameSound.Payer_Dash_Up, transform.position);
                dash_Up[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < dash.Length; i++)
            {
                GameManager.Instance.sM.PlaySoundPositioned(GameSound.Payer_Dash, transform.position);
                dash[i].Play();
            }
        }
    }

    public void FeedBackTakeDamage()
    {
        for (int i = 0; i < EnemyHit.Length; i++)
        {
            GameManager.Instance.sM.PlaySoundPositioned(GameSound.Payer_TakeDamage, transform.position);
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
                GameManager.Instance.sM.PlaySoundPositioned(GameSound.WaterOut, transform.position);
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
        GameManager.Instance.sM.PlaySoundPositioned(GameSound.WaterIn, transform.position);
        animWaterAbsorption.Play("WaterShield_Absorption");
    }

    public void playAttackSound()
    {
        if (SpecialAttack)
        {
            GameManager.Instance.sM.PlaySoundPositioned(GameSound.Payer_Attack_Up, transform.position);
        }
        else
        {
            GameManager.Instance.sM.PlaySoundPositioned(GameSound.Payer_Attack, transform.position);
        }
    }

    public void SmokeDeath()
    {
        GameObject newSmoke = Instantiate(GameManager.Instance.VFXSmoke, transform.position - transform.forward * 2f, Quaternion.identity , transform);
        newSmoke.GetComponent<ParticleSystem>().Play();
        GameManager.Instance.sM.PlaySoundPositioned(GameSound.Death_Smoke, transform.position);
    }
}
