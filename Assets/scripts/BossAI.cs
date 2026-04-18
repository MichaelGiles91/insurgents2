using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    
    public float bossSpeed;
    public Transform target;
    public float minumumDistance;

    //public GameObject projectile;
    public float timeBetweenShots;
    private float nextShotTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextShotTime)
        {
           // Instantiate(projectile, transform.position, Quaternion.identity);
            nextShotTime = Time.time + timeBetweenShots;
        }

        if (Vector3.Distance(transform.position, target.position) > minumumDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, bossSpeed * Time.deltaTime);
        }
        {
            //attack player
        }
    }

 
}

