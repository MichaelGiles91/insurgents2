using UnityEngine;

public class rotatePickup : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float floatHeight = 0.25f;
    [SerializeField] float floatSpeed = 2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // spin
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // float up and down
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}