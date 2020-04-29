using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class IABehaviour : MonoBehaviour, IDamageable
{
    public delegate void OnIAStateChangedDelegate(IABehaviour entity, IAState oldState, IAState newState);

    public NavMeshAgent navA;
    public MeshRenderer mR;
    public Material[] stateMaterials;
    public GameObject[] hitBoxVisualisation;

    public IAStats IAStats;
    public GameObject dropItem;
    public bool isTesting = true;

    public float AIperceptionUpdate;
    private Vector3 destiniation;

    public static OnIAStateChangedDelegate iaStateChangedDelegate;

    private IAState _currentIAState;
    public IAState currentIAState
    {
        get => _currentIAState;
        set
        {
            IAState before = _currentIAState;
            _currentIAState = value;
            ChangeMaterial((int)currentIAState);
            iaStateChangedDelegate(this, before, value);
        }
    }

    private int specialAttackWaiting;
    private float currentAttackCooldown;
    private bool attackInCooldown;
    private bool attackCanceled;
    private int currentLife;
    private float stunnedDuration;


    public bool isInvincible { get; private set; }
    public float invicibilitéDuration;


    private void Awake()
    {
        currentLife = IAStats.lifePointTypes.Length - 1;
        print(currentLife);
    }

    void Start()
    {
        AIPursuit(GameManager.Instance.player.transform);
    }

    private void Update()
    {
        if (isTesting && Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
            // TakeDamage(2);
        }
    }

    #region IA State

    public void ChangeMaterial(int index)
    {
        if(index - 1 >= 0)
        mR.material = stateMaterials[index-1];
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

        currentIAState = IAState.mooving;
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
        int indexToTake = attackIndex;

        if (specialAttackWaiting > 0)
        {
            specialAttackWaiting--;
            indexToTake = 1;
            print("Force special Attack");
        }

        attackCanceled = false;
        if (indexToTake < 0 || indexToTake > (IAStats.Attack.Length - 1))
        {
            print("BadIndex");
            return;
        }

        float prepDuration = IAStats.Attack[indexToTake].preparationDuration;
        float attackDuration = IAStats.Attack[indexToTake].attackRecoveryDuration;
        currentAttackCooldown = IAStats.Attack[indexToTake].attackCoolDownDuration;

        //Reortation 
        Vector3 directionToFace = GameManager.Instance.player.transform.position;
        Vector3 targetPos = new Vector3(directionToFace.x, transform.position.y, directionToFace.z);
        transform.LookAt(targetPos);

        //PREP
        currentIAState = IAState.attackPrep;

        hitBoxVisualisation[indexToTake].SetActive(true);
        StartCoroutine(attackDebugCooldown(prepDuration, attackDuration, IAStats.Attack[indexToTake].attackType));


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
                    StartCoroutine(Attack(ray, IAStats.Attack[0]));
                    break;
                case LifePointType.specialAttack:
                    // specialAttackWaiting--;
                    currentIAState = IAState.specialAttack;
                    StartCoroutine(Attack(ray, IAStats.Attack[1]));
                    break;
            }
            StartCoroutine(AttackCooldown());
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
    IEnumerator Attack(Ray ray, Attack attack)
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
            yield return null;
        }
        hitBoxVisualisation[(int)attack.attackType].SetActive(false);
        yield return new WaitForSeconds(attack.attackRecoveryDuration);
        AIPursuit(GameManager.Instance.player.transform);

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

    private void OnDead()
    {
        currentIAState = IAState.dead;
        navA.destination = transform.position;
        StopAllCoroutines();

        GameObject go = Instantiate(dropItem, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        Tokendo tokendo = go.GetComponent<Tokendo>();
        if (!tokendo.moveToPlayerAtStart)
        {
            tokendo.MoveToPlayer();
        }
    }

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
            OnDead();
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
                        hitBoxVisualisation[0].SetActive(false);
                        print("Prep Cancel");
                        break;

                    case IAState.attacking:
                        StopCoroutine("attackDebugCooldown");
                        StopCoroutine("AttackCooldown");
                        StopCoroutine("Attack");

                        hitBoxVisualisation[0].SetActive(false);
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

    public void GetStunned(float duration)
    {
        for (int i = 0; i < hitBoxVisualisation.Length; i++)
        {
            hitBoxVisualisation[i].SetActive(false);
        } 
        
        switch (currentIAState)
        {
            case IAState.mooving:
                StopCoroutine("AIPursuit");
                StopCoroutine("pursuitCooldown");
                navA.isStopped = true;

                break;
            case IAState.attackPrep:
                StopCoroutine("attackDebugCooldown");
                break;

            case IAState.attacking:
                StopCoroutine("attackDebugCooldown");
                StopCoroutine("AttackCooldown");
                attackCanceled = true;
                break;

            case IAState.specialAttack:
                StopCoroutine("attackDebugCooldown");
                StopCoroutine("AttackCooldown");
                specialAttackWaiting++;
                attackCanceled = true;
                break;

            case IAState.stunned:
                stunnedDuration += duration;
                return;
        }
        StunnedTimer(duration);

    }

    IEnumerator StunnedTimer(float duration)
    {
        bool loop = true;
        stunnedDuration += duration;
        while (loop)
        {
            stunnedDuration -= Time.deltaTime;
            if (0 >= stunnedDuration)
            {
                loop = false;
            }
            yield return null;
        }
        stunnedDuration = 0;
        AIPursuit(GameManager.Instance.player.transform);    
    }

    #region EDITOR
    [ContextMenu("GetNavMeshAgent")]
    private void GetNavMeshAgent()
    {
        navA = GetComponent<NavMeshAgent>();
    }

    [ContextMenu("QuickSetup")]
    private void QuickSetup()
    {
        navA = GetComponent<NavMeshAgent>();
        mR = GetComponent<MeshRenderer>();

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
    justSpawned,
    mooving,
    attackPrep,
    attacking,
    specialAttack,
    stunned,
    dead,
}