using UnityEngine;

public class objectiveMarker : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);

        if(screenPos.z < 0)
        {
            screenPos *= -1;
        }

        bool onScreen = screenPos.z > 0 &&
                        screenPos.x > 0 && screenPos.x < Screen.width &&
                        screenPos.y > 0 && screenPos.y < Screen.height;

        if (onScreen)
        {
            transform.position = screenPos;
        }
        else
        {
            screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
            screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);
            transform.position = screenPos;
        }
    }
}