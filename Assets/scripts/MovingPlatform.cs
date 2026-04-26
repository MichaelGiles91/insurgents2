using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform Platform;

    [SerializeField] float raisedHeight = 3f;
    [SerializeField] float speed = 2f;

    [SerializeField] TMP_Text Text;



    bool canPress;
    bool isRaised;

    Vector3 startPOS;
    Vector3 endPOS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPOS = Platform.position;
        endPOS = startPOS + Vector3.up * raisedHeight;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && canPress)
        {
            isRaised = !isRaised;
        }
        if (canPress)
        {
            if(isRaised)
            {
                Text.text = "Press E To Lower target";

            }
            else
            {
                Text.text = "Press E to Raise Target";
            }
        }
        Vector3 target;

        if (isRaised)
        {
            target = endPOS;
        }
        else
        {
            target = startPOS;
        }


        Platform.position = Vector3.Lerp(Platform.position, target, speed * Time.deltaTime);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(Platform);
        }
        if (other.CompareTag("Player"))
        {
            canPress = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(Platform);
        }
        if (other.CompareTag("Player"))
        {
            canPress = false;
           Text.gameObject.SetActive(false);
        }
    }
}
