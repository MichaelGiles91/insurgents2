using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Damage : MonoBehaviour
{      
    enum damageType
    {
        bullet, stationary, DOT
    }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] ParticleSystem hitEffect;

    bool isDamaged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(type == damageType.bullet)
        {
           
            Destroy(gameObject, destroyTime);
        }
    }

    //collider that we can pass thru like a wall.
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type != damageType.DOT)
        {
            dmg.takeDamage(damageAmount);
        }
        if(type == damageType.bullet)
        {
            //check to do everytime
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject,destroyTime);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type == damageType.DOT && !isDamaged)
        {
            StartCoroutine(damageOther(dmg));
        }
    }
    IEnumerator damageOther(IDamage d)
    {
        isDamaged = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaged = false;
    }
}


