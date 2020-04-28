using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(IAAttackGestion))]
public class IABehaviour : MonoBehaviour, IDamageable
{

    public PlayerController player;
    public NavMeshAgent navA;
    public MeshRenderer mR;
    public Material[] stateMaterials;

    public IAStats IAStats;

    public float AIperceptionUpdate;
    private Vector3 destiniation;
    private IAState currentIAState;

    private int specialAttackWaiting;
    private int currentLife;


    private void Awake()
    {
        currentLife = IAStats.lifePointTypes.Length - 1;
        print(currentLife);

    }

    void Start()
    {
        AIPursuit(player.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
            // TakeDamage(2);
        }
    }

    #region IA State

    public void IAChangeState(int stateIndex)
    {
        currentIAState = (IAState)stateIndex;
        ChangeMaterial(stateIndex);
    }

    public void IAChangeState()
    {
        ChangeMaterial((int)currentIAState);
    }

    public void ChangeMaterial(int index)
    {
        mR.material = stateMaterials[index];
    }

    #endregion

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
        Debug.DrawRay(transform.position, rotatedFroward * 10, Color.green, 1f);

        print(angleToAdd);
        return angleToAdd;
    }


    public bool AIPursuit(Transform pos)
    {
        if (navA.isStopped)
        {
            navA.isStopped = false;
        }

        IAChangeState(0);
        float distance = Vector3.Distance(transform.position, pos.position);
        AiMoveTo(pos.position);
        //print(distance);
        if (distance < 3)
        {
            //print("A cote de la cible");
            navA.destination = transform.position;
            IAAttack(0);
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
        if (currentIAState == IAState.mooving)
        {
            AIPursuit(pos);
        }
    }


    #endregion

    #region IA Attack


    public void IAAttack(int attackIndex)
    {

        if (attackIndex < 0 || attackIndex > (IAStats.Attack.Length - 1))
        {
            print("BadIndex");
            return;
        }
        float prepDuration = IAStats.Attack[attackIndex].prepDuration;
        float attackDuration = IAStats.Attack[attackIndex].prepDuration;



        //Reortation 
        Vector3 directionToFace = player.transform.position;
        Vector3 targetPos = new Vector3(directionToFace.x, transform.position.y, directionToFace.z);
        transform.LookAt(targetPos);

        IAChangeState(1);

        StartCoroutine(attackDebugCooldown(prepDuration, attackDuration, IAStats.Attack[attackIndex].attackTypes));


    }

    IEnumerator attackDebugCooldown(float prepDuration, float attackDuration, LifePointType attackType)
    {
        yield return new WaitForSeconds(prepDuration);
        switch (attackType)
        {
            case LifePointType.normal:
                currentIAState = IAState.attacking;
                IAChangeState();
                break;
            case LifePointType.specialAttack:
                print("Special Attack ");
                specialAttackWaiting--;
                currentIAState = IAState.specialAttack;
                IAChangeState();
                break;
        }

        yield return new WaitForSeconds(attackDuration);
        if (specialAttackWaiting > 0)
        {
            StopCoroutine("AIPursuit");
            StopCoroutine("pursuitCooldown");
            navA.isStopped = true;
            //IAAttack(1);
            IAAttack(1);
        }
        else
        {
            print("Poursuit Reprise");
            AIPursuit(player.transform);
        }

    }




    #endregion

    public void TakeDamage(int damageAmount)
    {
        int lastLife = currentLife;

        currentLife -= damageAmount;
        if (currentLife < 0)
        {
            currentIAState = IAState.dead;
            navA.destination = transform.position;
            IAChangeState();
            StopAllCoroutines();
            return;
        }
        print(currentLife);

        for (int i = 0; i < damageAmount; i++)
        {
            print("PV type : " + IAStats.lifePointTypes[lastLife - (1 + i)]);
            if (IAStats.lifePointTypes[lastLife - (1 + i)] == LifePointType.specialAttack)
            {
                if (currentIAState == IAState.mooving)
                {
                    StopCoroutine("AIPursuit");
                    StopCoroutine("pursuitCooldown");
                    navA.isStopped = true;
                    IAAttack(1);
                }
                else
                {
                    specialAttackWaiting++;
                    print("Add new stack for Super Attack");
                }
            }
        }

    }

    #region EDITOR
    [ContextMenu("FindPlayer")]
    private void FindPlayer()
    {
        player = FindObjectOfType<PlayerController>();
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
        player = FindObjectOfType<PlayerController>();
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

public enum IAState
{
    mooving,
    attackPrep,
    attacking,
    specialAttack,
    stunned,
    dead
}