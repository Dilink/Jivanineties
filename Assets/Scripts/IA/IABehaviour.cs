using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(IAAttackGestion))]
public class IABehaviour : MonoBehaviour
{

    public IAFakePlayer player;
    public NavMeshAgent navA;
    public MeshRenderer mR;

    private Vector3 destiniation;
    public float AIperceptionUpdate;


    void Start()
    {
        AIPursuit(player.transform);
    }



    #region IA mouvement
    public bool AiMoveTo(Vector3 pos, bool continuePattern = false)
    {
        navA.destination = pos;
        return false;
    }

    public float GetRotationAngleFromPos(Vector3 pos)
    {
        Vector3 directionToFace = pos - transform.position;


        directionToFace.y = 0;
        directionToFace = directionToFace.normalized;
        //Debug.DrawRay(transform.position, directionToFace * 3, Color.blue , 2f);
        float firstDot = Vector3.Dot(directionToFace, transform.forward);

        Vector3 rotatedDirection = Quaternion.Euler(0, 90, 0) * directionToFace;
        //Debug.DrawRay(transform.position, rotatedDirection * 3, Color.red , 2f);

        float secondDot = Vector3.Dot(rotatedDirection, transform.right);
        float angleToAdd;

        if (firstDot > 0)
        {
            if (secondDot > 0)
            {
                angleToAdd = 90 - 90 * Mathf.Abs(firstDot);
            }
            else
            {
                angleToAdd = -90 + 90 * Mathf.Abs(firstDot);
            }

        }
        else
        {
            if (secondDot > 0)
            {
                angleToAdd = 90 + 90 * Mathf.Abs(firstDot);
            }
            else
            {
                angleToAdd = -90 - 90 * Mathf.Abs(firstDot);
            }
        }
        Vector3 rotatedFroward = Quaternion.Euler(0, angleToAdd, 0) * transform.forward;
        Debug.DrawRay(transform.position, rotatedFroward * 10, Color.green , 1f);

        print(angleToAdd);
        return angleToAdd;
    }


    public bool AIPursuit(Transform pos)
    {

        float distance = Vector3.Distance(transform.position, pos.position);
        AiMoveTo(pos.position);
        //print(distance);
        if (distance < 3)
        {
            //print("A cote de la cible");
            navA.destination = transform.position;
            IAAttack(1f, 2f, 2);
        }
        else
        {
            StartCoroutine(pursuitCooldown(pos, AIperceptionUpdate));
        }


        return false;
    }
    IEnumerator pursuitCooldown(Transform pos, float duration)
    {
       // print("Poursuite en cours");
        yield return new WaitForSeconds(duration);
        AIPursuit(pos);
    }


    #endregion

    #region IA Attack


    public void IAAttack(float prepDuration, float AttackDuration, int attackRepetition = 1, int index = 0)
    {
        int _index = index;
        index++;
        //Reortation 
        //transform.forward = Quaternion.Euler(0f, GetRotationAngleFromPos(player.transform.position), 0) * transform.forward;
        //transform.rotation = transform.LookAt(player.transform);
        Vector3 directionToFace =player.transform.position;
        Vector3 targetPos = new Vector3(directionToFace.x, transform.position.y, directionToFace.z);
        transform.LookAt(targetPos);


        print("Prep");
        StartCoroutine(attackDebugCooldown(prepDuration, AttackDuration, _index, attackRepetition));
    }

    IEnumerator attackDebugCooldown(float prepDuration, float attackDuration, int index, int attackRepetition)
    {
        yield return new WaitForSeconds(prepDuration);
        print("Attack");
        yield return new WaitForSeconds(attackDuration);
        if (attackRepetition > index)
        {
            IAAttack(prepDuration, attackDuration, index, attackRepetition);
        }
        else
        {
            print("Poursuit Reprise");
            AIPursuit(player.transform);
        }

    }


    #endregion

    #region EDITOR
    [ContextMenu("FindPlayer")]
    private void FindPlayer()
    {
        player = FindObjectOfType<IAFakePlayer>();
        if (player != null)
        {
            print("success");
        }
        else
        {
            print("fail");
        }
    }

    [ContextMenu("GetNavMeshAgent")]
    private void GetNavMeshAgent()
    {
        navA = GetComponent<NavMeshAgent>();
        if (player != null)
        {
            print("success");
        }
        else
        {
            print("fail");
        }
    }

    [ContextMenu("QuickSetup")]
    private void QuickSetup()
    {
        player = FindObjectOfType<IAFakePlayer>();
        navA = GetComponent<NavMeshAgent>();
        mR = GetComponent<MeshRenderer>();
        if (player != null)
        {
            print(" P success");
        }
        else
        {
            print(" P fail");
        }

        if (navA != null)
        {
            print("N success");
        }
        else
        {
            print("N fail");
        }

        if (mR != null)
        {
            print("M success");
        }
        else
        {
            print("M fail");
        }
    }


    #endregion
}