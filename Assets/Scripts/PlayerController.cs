using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float pulseRadius = 5f;
    public float pulseForce = 10f;
    public GameObject pulseVisualPrefab;
    public CameraShake cameraShake;
    public float pulseCooldown = 2f;
    public float maxChargeTime = 3f;
    public float maxPulseForce = 20f;

    [SerializeField] GameObject pulseIndicator;
    [SerializeField] private Image pulseIndicatorImage;
    [SerializeField] private GameObject chargeIndicator;
    [SerializeField] private Image chargeIndicatorImage;

    private Rigidbody rb;
    private Vector2 moveInput;
    private float lastPulseTime;
    private PlayerControl controls;
    private bool isCharging = false;
    private float chargeTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControl();
        lastPulseTime = -pulseCooldown; // Allow pulse immediately at start
        pulseIndicator.SetActive(false);
        chargeIndicator.SetActive(false);

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Pulse.started += ctx => StartCharge();
        controls.Player.Pulse.canceled += ctx => ReleaseCharge();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    
    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (isCharging)
        {
            chargeTime += Time.fixedDeltaTime;
            chargeIndicator.SetActive(true);
            chargeIndicatorImage.fillAmount = Mathf.Clamp01(chargeTime / maxChargeTime);
        }
        else
        {
            chargeIndicator.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Move();

        if (Time.time < lastPulseTime + pulseCooldown)
        {
            pulseIndicator.SetActive(true);
            pulseIndicatorImage.fillAmount = (Time.time - lastPulseTime) / pulseCooldown;
        }
        else
        {
            pulseIndicator.SetActive(false);

        }
    }

    void Move()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.linearVelocity = movement * moveSpeed;
    }

    void GravityPulse(float force)
    {
        if (Time.time < lastPulseTime + pulseCooldown)
        {
            Debug.Log("Pulse on cooldown");
            return;
        }          
        lastPulseTime = Time.time;

        Collider[] hits = Physics.OverlapSphere(transform.position, pulseRadius);

        foreach (Collider hit in hits)
        {
            IPulseInteractable interactable = hit.GetComponent<IPulseInteractable>();

            if (interactable != null)
            {
                interactable.OnPulseInteract(transform.position, force, pulseRadius);
                Instantiate(pulseVisualPrefab, transform.position, Quaternion.identity);
                cameraShake.Shake();
            }
        }

        rb.AddForce(-transform.forward * pulseForce, ForceMode.Impulse);
    }

    void StartCharge()
    {
        if (Time.time < lastPulseTime + pulseCooldown)
        {
            Debug.Log("Pulse on cooldown");
            return;
        }
        isCharging = true;
        chargeTime = 0f;
        Debug.Log("Started charging pulse");
    }

    void ReleaseCharge()
    {
        if (!isCharging) return;
        isCharging = false;
        float chargePercent = Mathf.Clamp01(chargeTime / maxChargeTime);
        float force = Mathf.Lerp(pulseForce, maxPulseForce, chargePercent);
        Debug.Log(force);
        GravityPulse(force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, pulseRadius);
    }
}
