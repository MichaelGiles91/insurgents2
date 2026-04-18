using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // destroy on hit
        Destroy(gameObject);
    }
}