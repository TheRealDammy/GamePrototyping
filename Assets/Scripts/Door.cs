using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 closedPosition;
    private Vector3 openPosition;

    public float openHeight = 3f;
    public float openSpeed = 2f;

    private bool isOpen = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * openSpeed);

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * openSpeed);
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }
    public void CloseDoor()
    {
        isOpen = false;
    }
}
