using UnityEngine;

public class Create : MonoBehaviour, IPulseInteractable
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPulseInteract(Vector3 pulseOrigin, float pulseForce, float pulseRadius)
    {
        Vector3 direction = (transform.position - pulseOrigin).normalized;
        float distance = Vector3.Distance(transform.position, pulseOrigin);
        float forceMagnitude = Mathf.Lerp(pulseForce, 0, distance / pulseRadius);
        rb.AddForce(direction * forceMagnitude, ForceMode.Impulse);
    }
}
