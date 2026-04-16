using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] Transform model;
    [SerializeField] GameObject button;
    [SerializeField] TMP_Text closedText;
   

    [Header("Door Settings")]
    [SerializeField] float openAngle = 90f;
    [SerializeField] float speed = 2f;

    bool canOpen;
    bool isOpen;

    Quaternion closedRotation;
    Quaternion openRotation;

    private void Start()
    {
        closedRotation = model.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }


    void Update()
    {
        if (gameManager.instance != null && gameManager.instance.isPaused)
        {
            button.SetActive(false);
            return;
        }
        if (Input.GetButtonDown("Interact") && canOpen)
        {
            isOpen = !isOpen;
        }
        if (canOpen)
        {
            if (isOpen)
            {
                closedText.text = "Press E to Close";
            }
            else
            {
                closedText.text = "Press E to Open";

            }
        }
        if (isOpen)
        {
            model.rotation = Quaternion.Lerp(model.rotation, openRotation, speed * Time.deltaTime);

        }
        else
        {
            model.rotation = Quaternion.Lerp(model.rotation, closedRotation, speed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IOpen>() != null)
        {
            canOpen = true;
            button.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
            button.SetActive(false);
        }
    }

  
}
