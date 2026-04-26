using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform player;
    Vector3 Direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Direction.z = player.eulerAngles.y;
        transform.localEulerAngles = Direction;
    }
}
