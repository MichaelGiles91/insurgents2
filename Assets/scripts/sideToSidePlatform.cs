using UnityEngine;

public class sideToSidePlatform : MonoBehaviour
{
    [SerializeField] Transform Platform;

    [SerializeField] float Dir = 3f;
    [SerializeField] float speed = 2f;

    Vector3 startPOS;
    Vector3 rightPOS;
    Vector3 leftPOS;
    private Vector3 lastPosition;
    private Vector3 velocity;

    int targetIndex = 0;
    Vector3[] targets; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPOS = Platform.position;
        rightPOS = startPOS + Vector3.right * Dir;
        leftPOS = startPOS + Vector3.left * Dir;

        targets = new Vector3[]
        {
            rightPOS, startPOS, leftPOS,startPOS
        };
    }

    // Update is called once per frame
    void Update()
    {
        velocity = (Platform.position - lastPosition) / Time.deltaTime;
        lastPosition = Platform.position;
        MovePlatform();
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    void MovePlatform()
    {
        Platform.position = Vector3.MoveTowards(Platform.position, targets[targetIndex], speed * Time.deltaTime);

        if(Vector3.Distance(Platform.position, targets[targetIndex]) < 0.05f)
        {
            targetIndex++;

            if(targetIndex >= targets.Length)
            {
                targetIndex = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(Platform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
