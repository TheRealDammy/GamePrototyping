
using UnityEngine;

public class PulseVisual : MonoBehaviour
{
    public float expansionSpeed = 5f;
    public float maxScale = 10f;

    void Update()
    {
        transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;
        if (transform.localScale.x >= maxScale)
        {
            Destroy(gameObject);
        }
    }
}
