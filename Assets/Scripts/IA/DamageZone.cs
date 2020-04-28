using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private  GameObject _parent;
    public int  damageDealed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _parent)
        {
           IDamageable target =   other.GetComponent<IDamageable>();
            if (target !=null)
            {

                target.TakeDamage(damageDealed);
            }

        }
    }

    public void Init(GameObject parent)
    {
        _parent = parent;
    }
}
