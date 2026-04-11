using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{

    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;

    float xRotation = 0f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        look();
    }


    void look()
    {
        Vector2 mouseInput = Mouse.current.delta.ReadValue();

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

}