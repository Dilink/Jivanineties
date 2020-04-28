using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour, IDamageable
{
    [Header("Tweaking")]
    [Range(0, 50)]
    public float moveSpeed = 5f;
    public AnimationCurve dodgeCurve;
    [Range(0, 5)]
    public float collisionDetectionRange = 1f;
    //public Vector3 collisionDetectionBox;
    public Attack standardAttack; 

    [Header("References")]
    public Transform visual;
    public AbsorptionController absorption;
    
    private Vector3 movement;
    private float speedModifier;
    private float movementModifier;
    private Coroutine dodging;
    private Coroutine attacking;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        speedModifier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        CheckMovement();
        CheckDodge();
        //CheckCollisions();
        Move();
    }

    private void CheckAttack()
    {
        if(dodging == null && InputManager.Instance.ATTACK && attacking == null)
        {
            attacking = StartCoroutine(Attack(new Ray(transform.position + Vector3.up, visual.forward), standardAttack));
        }
    }

    private void CheckMovement()
    {
        if(dodging == null && attacking == null)
        {
            movement = Vector3.zero;
            if(InputManager.Instance.UP)
            {
                movement += new Vector3(0, 0, 1);
            }
            else if(InputManager.Instance.DOWN)
            {
                movement += new Vector3(0, 0, -1);
            }
            if(InputManager.Instance.RIGHT)
            {
                movement += new Vector3(1, 0, 0);
            }
            else if(InputManager.Instance.LEFT)
            {
                movement += new Vector3(-1, 0, 0);
            }
            movement = movement.normalized;
        }
    }
    
    private void CheckDodge()
    {
        if(InputManager.Instance.DODGE && dodging == null && attacking == null)
        {
            dodging = StartCoroutine(Dodge());
        }
    }

    /*
    private void CheckCollisions()
    {
        if(movement != Vector3.zero)
        {
            Ray ray = new Ray(transform.position + Vector3.up, movement);
            RaycastHit hit;
            ExtDebug.DrawBoxCastBox(ray.origin, collisionDetectionBox / 2f, visual.rotation, ray.direction, 5f, Color.blue);
            if(Physics.BoxCast(ray.origin, new Vector3(collisionDetectionBox.x / 2f, collisionDetectionBox.y / 2f, 0), ray.direction, out hit, visual.rotation, 5f, 1 << LayerMask.NameToLayer("Obstacle")))
            {
                movementModifier = Vector3.Distance(ray.origin, hit.point);
                if(movementModifier < collisionDetectionBox.z)
                {
                    movementModifier = 0;
                    return;
                }
            }
            movementModifier = moveSpeed * speedModifier * Time.deltaTime;
        }
    }
    */

    private void Move()
    {
        movementModifier = moveSpeed * speedModifier * Time.deltaTime;
        transform.position += movement * movementModifier;
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
                Debug.Log("Hit!");
                enemyHit = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(attack.attackRecoveryDuration);
        attacking = null;
    }

    IEnumerator Dodge()
    {
        float timer = 0f;
        do
        {
            speedModifier = dodgeCurve.Evaluate(timer);
            timer += Time.deltaTime;
            yield return null;
        }
        while(timer < dodgeCurve.keys[dodgeCurve.length - 1].time);
        speedModifier = 1f;
        movement = Vector3.zero;
        dodging = null;
    }

    public void TakeDamage(int damageAmount)
    {
        if(!absorption.TryAbsorption())
        {
            Debug.Log("Aïe! J'ai mal!");
        }

    }
}
