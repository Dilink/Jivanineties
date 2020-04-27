using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    [Header("Tweaking")]
    public float moveSpeed = 5f;
    public float dodgeAmplitude = 5f;
    public float dodgeDuration = 0.2f;
    public float dodgeRecoveryDuration = 0.1f;
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
            StartCoroutine(Attack());
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
        if(InputManager.Instance.DODGE && dodging == null)
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

    IEnumerator Dodge()
    {
        speedModifier = dodgeAmplitude;
        yield return new WaitForSeconds(dodgeDuration);
        speedModifier = 1f;
        movement = Vector3.zero;
        yield return new WaitForSeconds(dodgeRecoveryDuration);
        dodging = null;
    }
}
