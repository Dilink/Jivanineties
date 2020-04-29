using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class IABehaviour : MonoBehaviour, IDamageable
{

    public PlayerController player;
    public NavMeshAgent navA;
    public MeshRenderer mR;
    public Material[] stateMaterials;
    //public Attack[] damageZones;

    public IAStats IAStats;

    public float AIperceptionUpdate;
    private Vector3 destiniation;
    private IAState currentIAState;

    private int specialAttackWaiting;
    private float currentAttackCooldown;
    private bool attackInCooldown;
    private bool attackCanceled;
    private int currentLife;


    public bool isInvincible { get; private set; }
    public float invicibilitéDuration;


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
        if (distance < 2 && !attackInCooldown)
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

        attackCanceled = false;
        if (attackIndex < 0 || attackIndex > (IAStats.Attack.Length - 1))
        {
            print("BadIndex");
            return;
        }
        float prepDuration = IAStats.Attack[attackIndex].preparationDuration;
        float attackDuration = IAStats.Attack[attackIndex].attackRecoveryDuration;
        currentAttackCooldown = IAStats.Attack[attackIndex].attackCoolDownDuration;

        //Reortation 
        Vector3 directionToFace = player.transform.position;
        Vector3 targetPos = new Vector3(directionToFace.x, transform.position.y, directionToFace.z);
        transform.LookAt(targetPos);

        //PREP
        IAChangeState(1);

        StartCoroutine(attackDebugCooldown(prepDuration, attackDuration, IAStats.Attack[attackIndex].attackType));


    }

    IEnumerator attackDebugCooldown(float prepDuration, float attackDuration, LifePointType attackType)
    {
        if (attackCanceled)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(prepDuration);
            Ray ray = new Ray(transform.position, transform.forward);
            switch (attackType)
            {
                case LifePointType.normal:
                    currentIAState = IAState.attacking;
                    //damageZones[0].gameObject.SetActive(true);
                    Attack(ray, IAStats.Attack[0]);
                    IAChangeState();
                    break;
                case LifePointType.specialAttack:
                    specialAttackWaiting--;
                    currentIAState = IAState.specialAttack;
                    Attack(ray, IAStats.Attack[1]);
                    IAChangeState();
                    break;
            }
            StartCoroutine(AttackCooldown());
            yield return new WaitForSeconds(attackDuration);
            if (attackCanceled)
            {
                yield return null;
            }
            else
            {
                AIPursuit(player.transform);
            }
        }

        //if (specialAttackWaiting > 0)
        //{
        //    StopCoroutine("AIPursuit");
        //    StopCoroutine("pursuitCooldown");
        //    navA.isStopped = true;
        //    //IAAttack(1);
        //    IAAttack(1);
        //}


       // AIPursuit(player.transform);

    }

    private void Attack(Ray ray, Attack attack)
    {
        bool loop = true;
        bool enemyHit = false;
        float timer = 0f;
        while (loop && !attackCanceled)
        {
            timer += Time.deltaTime;
            if (timer >= attack.hitBoxDuration)
            {
                loop = false;
            }
            Collider[] enemies = Physics.OverlapBox(ray.origin + ray.direction * attack.rangeBox.z / 2f, attack.rangeBox / 2f, transform.rotation, 1 << LayerMask.NameToLayer("Player"));
            ExtDebug.DrawBoxCastBox(ray.origin, new Vector3(attack.rangeBox.x / 2f, attack.rangeBox.y / 2f, 0), transform.rotation, ray.direction, attack.rangeBox.z, Color.green);
            if (!enemyHit && enemies.Length > 0)
            {
                enemies[0].GetComponent<IDamageable>()?.TakeDamage(attack.damage);
                // Debug.Log("HIt heros");
                enemyHit = true;
            }
        }
    }
    IEnumerator AttackCooldown()
    {
        attackInCooldown = true;
        yield return new WaitForSeconds(currentAttackCooldown);
        attackInCooldown = false;
        //  print("no more cd on attack");
    }
    IEnumerator InvicibilityDuration()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invicibilitéDuration);
        isInvincible = false;
        print("no more  Invincible");
    }


    #endregion

    public void TakeDamage(int damageAmount)
    {
        if (isInvincible)
        {
            print("jsui invisible");
            return;
        }
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
        //print(currentLife);
        StartCoroutine(InvicibilityDuration());


        for (int i = 0; i < damageAmount; i++)
        {
            // print("PV type : " + IAStats.lifePointTypes[lastLife - (1 + i)]);
            if (IAStats.lifePointTypes[lastLife - (1 + i)] == LifePointType.specialAttack)
            {
                switch (currentIAState)
                {
                    case IAState.mooving:
                        StopCoroutine("AIPursuit");
                        StopCoroutine("pursuitCooldown");
                        navA.isStopped = true;
                        IAAttack(1);

                        break;
                    case IAState.attackPrep:
                        StopCoroutine("attackDebugCooldown");
                        IAAttack(1);
                        print("Prep Cancel");
                        break;

                    case IAState.attacking:
                        StopCoroutine("attackDebugCooldown");
                        StopCoroutine("AttackCooldown");

                        print("Attack  Cancel");
                        attackCanceled = true;
                        IAAttack(1);
                        break;

                        //case IAState.specialAttack:
                        //    StopCoroutine("attackDebugCooldown");
                        //    attackCanceled = true;
                        //    break;

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
    dead,
}