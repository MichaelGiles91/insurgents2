using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject fireBall;
    [SerializeField] Transform shootPos;
    [SerializeField] Renderer model;
   
    [SerializeField]int bossHP;
    [SerializeField]int bossSpeed;
    [SerializeField] float shootRate;
    [SerializeField] float jumpRate;
    [SerializeField] float jumpForce;

    Rigidbody rb;
    float jumpTimer;
   

    public Transform target;
    public float minumumDistance;

    float shootTimer;
    Color bossColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossColor = model.material.color;
        //rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        jumpTimer += Time.deltaTime;

        agent.SetDestination(gameManager.instance.player.transform.position);
        
        if (shootTimer >= shootRate)
        {
           shoot();
            
        }
       // if(jumpTimer >= jumpRate)
       // {
          // bossJump();
        //}
    

    }

    void shoot()
    {
        shootTimer = 0;
        Instantiate(fireBall, shootPos.position, transform.rotation);
    }

/* void bossJump()
    {
        
        jumpTimer = 0;
      
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }
*/
    public void takeDamage(int amount)
    {
        bossHP -=amount;
        if(bossHP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());

        }
    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = bossColor;
    }

}

