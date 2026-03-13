using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private CameraFollow follow;

    void Start()
    {
        originalPosition = transform.localPosition;
        follow = GetComponent<CameraFollow>();
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
        Debug.Log("Camera shake triggered!");
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float z = Random.Range(-1f, 1f) * shakeMagnitude;
            follow.SetShakeOffset(new Vector3(x, 0f, z));
            elapsed += Time.deltaTime;
            yield return null;
        }
        follow.SetShakeOffset(Vector3.zero);
    }
}
