using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject nextRoom; // Reference to the next room to activate

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (nextRoom != null)
            {
                nextRoom.SetActive(true); // Activate the next room
            }
            else
            {
                Debug.LogWarning("Next room reference is not set on " + gameObject.name);
            }
        }
    }
}
