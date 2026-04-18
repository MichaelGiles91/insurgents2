using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject fireBall;
    [SerializeField] Transform shootPos;
   
    [SerializeField]int bossHP;
    [SerializeField]int bossSpeed;
    [SerializeField] float shootRate;


    public Transform target;
    public float minumumDistance;

    float shootTimer;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        ////shootTimer += Time.deltaTime;

        ////if (Vector3.Distance(transform.position, target.position) > minumumDistance)
        ////{
        ////    transform.position = Vector3.MoveTowards(transform.position, target.position, bossSpeed * Time.deltaTime);
        ////    shoot();
        ////}
        ////else
        ////{

        ////}
        ///
        shootTimer += Time.deltaTime;
        agent.SetDestination(gameManager.instance.player.transform.position);
        {
            if (shootTimer >= shootRate)
            {
                shoot();
            }
        }
    }

    void shoot()
    {
        shootTimer = 0;
        Instantiate(fireBall, shootPos.position, transform.rotation);
    }

 
}

