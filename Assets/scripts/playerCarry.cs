using UnityEngine;

public class playerCarry : MonoBehaviour
{
    private Transform OGparent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OGparent = other.transform.parent;
            other.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PLayer"))
        {
            other.transform.SetParent(OGparent);
        }
    }
}
