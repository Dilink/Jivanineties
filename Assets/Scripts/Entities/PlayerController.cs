using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    [Header("Tweaking")]
    [Range(1, 50)]
    public float moveSpeed = 5f;
    public AnimationCurve dodgeCurve;
    //public float dodgeAmplitude = 5f;
    //public float dodgeDuration = 0.2f;
    //public float dodgeRecoveryDuration = 0.1f;
    [Range(0, 5)]
    public float collisionDetectionRange = 1f;
    public Attack standardAttack; 

    [Header("References")]
    public Transform visual;

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
        CheckCollisions();
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
        }
    }
    
    private void CheckDodge()
    {
        if(InputManager.Instance.DODGE && dodging == null && attacking == null)
        {
            dodging = StartCoroutine(Dodge());
        }
    }

    private void CheckCollisions()
    {
        Ray ray = new Ray(transform.position + Vector3.up, movement);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction);
        if(Physics.Raycast(ray, out hit, 10, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            movementModifier = Vector3.Distance(ray.origin, hit.point);
            Debug.Log(movementModifier);
            if(movementModifier < collisionDetectionRange)
            {
                movementModifier = 0;
                return;
            }
        }
        movementModifier = moveSpeed * speedModifier * Time.deltaTime;
    }

    private void Move()
    {
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
        RaycastHit hit;
        movement = Vector3.zero;
        while(loop)
        {
            timer += Time.deltaTime;
            if(timer >= attack.hitBoxDuration)
            {
                loop = false;
            }
            //bool hasHit = Physics.BoxCast(ray.origin, new Vector3(attack.rangeBox.x / 2f, attack.rangeBox.y / 2f, 0), ray.direction, out hit, visual.rotation, attack.rangeBox.z, 1 << LayerMask.NameToLayer("Enemy"))
            Collider[] enemies = Physics.OverlapBox(ray.origin + ray.direction * attack.rangeBox.z / 2f, attack.rangeBox / 2f, visual.rotation, 1 << LayerMask.NameToLayer("Enemy"));
            ExtDebug.DrawBoxCastBox(ray.origin, new Vector3(attack.rangeBox.x / 2f, attack.rangeBox.y / 2f, 0), visual.rotation, ray.direction, attack.rangeBox.z, Color.green);
            if(!enemyHit && enemies.Length > 0)
            {
                // Damage Enemy
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
        /*
        speedModifier = dodgeAmplitude;
        yield return new WaitForSeconds(dodgeDuration);
        speedModifier = 1f;
        movement = Vector3.zero;
        yield return new WaitForSeconds(dodgeRecoveryDuration);
        dodging = null;
        */
    }

}
