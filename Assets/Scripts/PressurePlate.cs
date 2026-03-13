using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Door connectedDoor;
    private Vector3 startPosition;

    private int objectsOnPlate = 0;

    void Start()
    {
        startPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crate"))
        {
            objectsOnPlate++;
            CheckPlate();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crate"))
        {
            objectsOnPlate--;
            CheckPlate();
        }
    }

    void CheckPlate()
    {
        if (objectsOnPlate > 0)
        {
            connectedDoor.OpenDoor();
            transform.position = startPosition + Vector3.down * 0.1f; // Press down the plate
        }
        else
        {
            connectedDoor.CloseDoor();
            transform.position = startPosition; // Reset plate position
        }
    }
}
