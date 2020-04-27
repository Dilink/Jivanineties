using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{

    public float moveSpeed = 5f;
    public float dodgeAmplitude = 1.01f;
    public float dodgeDuration = 0.1f;

    private Vector3 movement;
    private float speedModifier;
    private Coroutine dodging;

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        speedModifier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
        CheckDodge();
        Move();
    }

    private void CheckMovement()
    {
        if(dodging == null)
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

    private void Move()
    {
        transform.position += (movement * moveSpeed * speedModifier * Time.deltaTime);
    }

    IEnumerator Dodge()
    {
        speedModifier = dodgeAmplitude;
        yield return new WaitForSeconds(dodgeDuration);
        speedModifier = 1f;
        dodging = null;
    }
}
