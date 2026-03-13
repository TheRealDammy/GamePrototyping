using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, IPulseInteractable
{
    public Transform player;
    public Transform[] patrolPoints;
    public float moveSpeed = 3f;
    public float stunDuration = 2f;
    public float damage = 1f;
    public float detectionRange = 10f;
    public float visionAngle = 120f;

    private Renderer render;
    private Color originalColor;
    private Rigidbody rb;
    private bool isStunned = false;
    private int patrolIndex = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
        if (render != null)
        {
            originalColor = render.material.color;
        }
    }

    void FixedUpdate()
    {
        if (isStunned)
        {
            return;
        }
        else if (IsPlayerInSight())
        {
            MoveTowardsPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep movement on the horizontal plane
        transform.forward = direction;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    bool IsPlayerInSight()
    {
        if (player == null) return false;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRange) return false;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > visionAngle / 2) return false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer, out hit, detectionRange))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }

    public void OnPulseInteract(Vector3 pulseOrigin, float pulseForce, float pulseRadius)
    {
        if (isStunned) return;
        Vector3 direction = (transform.position - pulseOrigin).normalized;
        float distance = Vector3.Distance(transform.position, pulseOrigin);
        float forceMagnitude = Mathf.Lerp(pulseForce, 0, distance / pulseRadius);
        rb.linearVelocity = Vector3.zero; // Reset current velocity before applying impulses
        rb.AddForce(direction * forceMagnitude, ForceMode.Impulse);
        StartCoroutine(StunCoroutine());
        if (render != null)
        {
            StartCoroutine(HitFlash());
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[patrolIndex];

        Vector3 direction = (targetPoint.position - transform.position).normalized;
        direction.y = 0; // Keep movement on the horizontal plane
        transform.forward = direction;

        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

        if (direction.magnitude < 0.5f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Enemy hit the player!");
            }
        }
    }

    IEnumerator StunCoroutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    IEnumerator HitFlash()
    {
        render.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        render.material.color = originalColor;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Vector3 forward = transform.forward * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * forward;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
    }
}
