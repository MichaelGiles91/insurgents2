using UnityEngine;

public class objectiveMarker : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public static objectiveMarker instance;

    

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);

     
        if (screenPos.z < 0)
        {
            screenPos *= -1;
        }

    
        screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
        screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

        transform.position = screenPos;
    }
}
