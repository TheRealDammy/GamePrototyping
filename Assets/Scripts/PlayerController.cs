using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float pulseRadius = 5f;
    public float pulseForce = 10f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private PlayerControl controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControl();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Pulse.performed += ctx => GravityPulse();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.linearVelocity = movement * moveSpeed;
    }

    void GravityPulse()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pulseRadius);

        foreach (Collider hit in hits)
        {
            Rigidbody objRb = hit.GetComponent<Rigidbody>();
            if (objRb != null && objRb != rb)
            {
                Vector3 direction = hit.transform.position - transform.position;

                float distance = direction.magnitude;
                float strength = 1 - (distance / pulseRadius);

                objRb.AddForce(direction.normalized * pulseForce * strength, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, pulseRadius);
    }
}
