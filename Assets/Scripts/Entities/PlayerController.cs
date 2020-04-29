using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController: MonoBehaviour, IDamageable
{
    [Header("Tweaking")]
    public int hp = 1;
    [Range(0, 50)]
    public float moveSpeed = 5f;
    public AnimationCurve dodgeCurve;
    public float upgradedDodgeDuration;
    public float upgradedDodgeRecoveryDuration;
    public float upgradedDodgeRange;
    [Range(0, 10)]
    public float dodgeCooldownDuration;
    [Range(0, 5)]
    public float restoreDelay;

    public Attack standardAttack;
    public Attack upgradedAttack;

    [Header("References")]
    public Transform visual;
    public AbsorptionController absorption;
    public Animator animator;
    
    private Vector3 movement;
    private float speedModifier;
    private float movementModifier;
    private Coroutine dodging;
    private Coroutine attacking;
    private Coroutine restoring;

    private MeshRenderer mesh;

    private float dodgeCooldown;
    private float standardAttackCooldown;
    private float upgradedAttackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        speedModifier = 1f;
        mesh = visual.GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldowns();
        CheckRestore();
        CheckAttack();
        CheckMovement();
        CheckDodge();
        Move();
    }

    private void UpdateCooldowns()
    {
        dodgeCooldown = Mathf.Max(0, dodgeCooldown - Time.deltaTime);
        standardAttackCooldown = Mathf.Max(0, standardAttackCooldown - Time.deltaTime);
        upgradedAttackCooldown = Mathf.Max(0, upgradedAttackCooldown - Time.deltaTime);
    }

    private void CheckRestore()
    {
        if(dodging == null && attacking == null && restoring == null && GameManager.Instance.tokendoCount > 0 && GameManager.Instance.inputManager.POWER_HOLD && GameManager.Instance.combatController.currentPhase.ReturnType() == CombatPhaseType.WaitPhase)
        {
            restoring = StartCoroutine(Restore());
        }
    }

    private void CheckAttack()
    {
        if (dodging == null && restoring == null && GameManager.Instance.inputManager.ATTACK && attacking == null)
        {
            if(upgradedAttackCooldown <= 0 && GameManager.Instance.inputManager.POWER_HOLD && absorption.TryAbsorption())
            {
                attacking = StartCoroutine(Attack(new Ray(transform.position + Vector3.up, visual.forward), upgradedAttack));
                upgradedAttackCooldown = upgradedAttack.attackCoolDownDuration;
            }
            else if(standardAttackCooldown <= 0)
            {
                attacking = StartCoroutine(Attack(new Ray(transform.position + Vector3.up, visual.forward), standardAttack));
                standardAttackCooldown = standardAttack.attackCoolDownDuration;
            }
        }
    }

    private void CheckMovement()
    {
        if(dodging == null && attacking == null && restoring == null)
        {
            movement = Vector3.zero;
            if(GameManager.Instance.inputManager.UP)
            {
                movement += new Vector3(0, 0, 1);
            }
            else if(GameManager.Instance.inputManager.DOWN)
            {
                movement += new Vector3(0, 0, -1);
            }
            if(GameManager.Instance.inputManager.RIGHT)
            {
                movement += new Vector3(1, 0, 0);
            }
            else if(GameManager.Instance.inputManager.LEFT)
            {
                movement += new Vector3(-1, 0, 0);
            }
            movement = movement.normalized;
            if(movement.magnitude > 0)
            {
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }
        }
    }
    
    private void CheckDodge()
    {
        if (dodgeCooldown <= 0 && GameManager.Instance.inputManager.DODGE && dodging == null && attacking == null && restoring == null)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, upgradedDodgeRange, 1 << LayerMask.NameToLayer("Enemy"));
            if(enemies.Length > 0 && GameManager.Instance.inputManager.POWER_HOLD && absorption.TryAbsorption())
            {
                IABehaviour ia = enemies[0].GetComponent<IABehaviour>();
                if (ia)
                {
                    ia.currentIAState = IAState.stunned;
                }
                dodging = StartCoroutine(Dodge(enemies[0].transform));
            }
            else
            {
                dodging = StartCoroutine(Dodge(null));
            }
            dodgeCooldown = dodgeCooldownDuration;
        }
    }

    private void Move()
    {
        movementModifier = moveSpeed * speedModifier * Time.deltaTime;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + movement * movementModifier, out hit, 10f, 1 << NavMesh.GetAreaFromName("Walkable"));

        if (hit.position.x != Mathf.Infinity)
        {
            transform.position = hit.position;
        }

        if(!movement.Equals(Vector3.zero))
        {
            visual.rotation = Quaternion.LookRotation(movement, Vector3.up);
        }
    }

    IEnumerator Attack(Ray ray, Attack attack)
    {
        bool loop = true;
        bool enemyHit = false;
        float timer = 0f;
        movement = Vector3.zero;
        animator.SetTrigger("Attack");
        while(loop)
        {
            timer += Time.deltaTime;
            if(timer >= attack.hitBoxDuration)
            {
                loop = false;
            }
            Collider[] enemies = Physics.OverlapBox(ray.origin + ray.direction * attack.rangeBox.z / 2f, attack.rangeBox / 2f, visual.rotation, 1 << LayerMask.NameToLayer("Enemy"));
            ExtDebug.DrawBoxCastBox(ray.origin, new Vector3(attack.rangeBox.x / 2f, attack.rangeBox.y / 2f, 0), visual.rotation, ray.direction, attack.rangeBox.z, Color.green);
            if(!enemyHit && enemies.Length > 0)
            {
                enemies[0].GetComponent<IDamageable>()?.TakeDamage(attack.damage);
                if(attack.stunDuration > 0)
                {
                    IABehaviour ia = enemies[0].GetComponent<IABehaviour>();
                    if (ia)
                    {
                        ia.currentIAState = IAState.stunned;
                    }
                }
                Debug.Log("Hit!");
                enemyHit = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(attack.attackRecoveryDuration);
        attacking = null;
    }

    IEnumerator Dodge(Transform destination)
    {
        float timer = 0f;
        if(destination != null)
        {
            movement = Vector3.zero;
            NavMeshHit hit;
            NavMesh.SamplePosition(destination.position + (destination.position - transform.position).normalized * 2, out hit, 10f, 1 << NavMesh.GetAreaFromName("Walkable"));
            if(mesh != null)
            {
                mesh.enabled = false;
            }
            yield return new WaitForSeconds(upgradedDodgeDuration);
            transform.position = hit.position;
            if(mesh != null)
            {
                mesh.enabled = true;
            }
            yield return new WaitForSeconds(upgradedDodgeRecoveryDuration);
        }
        else
        {
            animator.SetTrigger("Dash");
            movement = visual.forward;
            do
            {
                speedModifier = dodgeCurve.Evaluate(timer);
                timer += Time.deltaTime;
                yield return null;
            }
            while(timer < dodgeCurve.keys[dodgeCurve.length - 1].time);
            speedModifier = 1f;
            movement = Vector3.zero;
        }
        dodging = null;
    }

    IEnumerator Restore()
    {
        float timer = 0f;
        bool loop = true;
        while(loop && GameManager.Instance.inputManager.POWER_HOLD)
        {
            if(timer >= restoreDelay)
            {
                loop = false;
            }
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        if(GameManager.Instance.inputManager.POWER_HOLD)
        {
            if(absorption.TryRestore())
            {
                GameManager.Instance.tokendoCount--;
            }
        }
        restoring = null;
    }

    public void TakeDamage(int damageAmount)
    {
        if(!absorption.TryAbsorption())
        {
            Debug.Log("Aïe! J'ai mal!");
            hp -= damageAmount;
            if(hp <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Je suis mort!");

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, upgradedDodgeRange);
    }
}
