using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{

    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpTimesMax;
    [SerializeField] int gravity;

    int jumpCount;
    int HPOrig;



    float shootTimer;

    Vector3 moveDir;
    Vector3 playerVel;


    void Start()
    {
        HPOrig = HP;
    }


    void Update()
    {
        movement();
    }




    void movement()
    {
        if (controller.isGrounded && playerVel.y <= 0)
        {
            playerVel.y = -5f;
            jumpCount = 0;
        }

        float x = 0;
        float z = 0;

        if (Keyboard.current.aKey.isPressed)
            x = -1;
        if (Keyboard.current.dKey.isPressed)
            x = 1;

        if (Keyboard.current.wKey.isPressed)
            z = 1;
        if (Keyboard.current.sKey.isPressed)
            z = -1;

        moveDir = x * transform.right + z * transform.forward;

        controller.Move(moveDir * speed * Time.deltaTime);
        if (controller.isGrounded)
        {
            jumpCount = 0;

            if (playerVel.y < 0)
                playerVel.y = -5f; 
        }


        // jump (space key)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpCount < jumpTimesMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }


        // gravity
        playerVel.y -= gravity * Time.deltaTime;

        controller.Move(playerVel * Time.deltaTime);
    }

}