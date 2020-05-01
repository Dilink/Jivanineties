using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{
    public Animator stateAnim;

    public ParticleSystem StunFX;
   // public Ma_SoundManager sM;

    public bool IsStun { get; set; }
    public bool isInvicible { get; set; }

    public void feedBackInvicibility()
    {
        //sM.PlaySoundPositioned(GameSound.Enemy_takeDamage, transform.position);
        if (IsStun)
        {
            feedBackStunInvisibility();
        }
        else
        {
            
            stateAnim.Play("Anim_Invincibility_2", -1 , 0);
        }

    }
    public void feedBackStun()
    {

        GameManager.Instance.sM.PlaySoundPositioned(GameSound.Enemy_stun, transform.position);
        if (isInvicible)
        {
            feedBackStunInvisibility();
        }
        else
        {
            stateAnim.Play("Anim_Stunned", -1, 0);
        }
    }

    public void feedBackStunInvisibility()
    {
        stateAnim.Play("Anim_Stunned_Invincibility", -1, 0);
    }

    public void endStun()
    {
        if (isInvicible)
        {
            //stateAnim.Play("Anim_Invincibility_2", -1, 0);
        }
        else
        {
            stateAnim.Play("New State", -1, 0);
        }
    }

    public void endInvincibility()
    {
        if (IsStun)
        {
            stateAnim.Play("Anim_Stunned", -1, 0);
        }
        else
        {
            stateAnim.Play("New State", -1, 0);
        }
    }

    public void SmokeDeath()
    {
        GameObject newSmoke = Instantiate(GameManager.Instance.VFXSmoke, transform.position + -transform.forward*2f, Quaternion.identity, transform);
       // newSmoke.transform.localScale = transform.parent.transform.localScale * 2;
        newSmoke.GetComponent<ParticleSystem>().Play();
        GameManager.Instance.sM.PlaySoundPositioned(GameSound.Death_Smoke, transform.position);
    }
}
