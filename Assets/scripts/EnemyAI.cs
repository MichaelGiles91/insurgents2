using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int meleeDamage;
    [SerializeField] float attackRate = 1f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] Animator animate;
    [SerializeField] int animateTransitionSpeed;

    float attackTimer;
    Transform player;
    float distance;

    Color colorOrig;
    Vector3 playerDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        locomotionAnim();

        attackTimer += Time.deltaTime;
        playerDirection = (gameManager.instance.player.transform.position - transform.position);

        agent.SetDestination(gameManager.instance.player.transform.position);

        distance = Vector3.Distance(transform.position, player.position);


        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            faceTarget();
        }
        if (distance <= attackRange && attackTimer >= attackRate)
        {
            attack();

        }
    }
    void locomotionAnim()
    {
        float agentCurrentSpeed = agent.velocity.normalized.magnitude;
        float agentSpeedAnim = animate.GetFloat("Speed");

        animate.SetFloat("Speed", Mathf.MoveTowards(agentSpeedAnim,agentCurrentSpeed, Time.deltaTime * animateTransitionSpeed));
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);


    }

    void attack()
    {
        attackTimer = 0;

        IDamage damageable = player.GetComponent<IDamage>();

        if (damageable != null)
        {
            damageable.takeDamage(meleeDamage);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && attackTimer >= attackRate)
        {
            IDamage damageable = other.GetComponent<IDamage>();

            if (damageable != null)
            {
                damageable.takeDamage(meleeDamage);
                attackTimer = 0;
                attack();
            }
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
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
        model.material.color = colorOrig;
    }

}