using UnityEngine;

public interface IPulseInteractable
{
    void OnPulseInteract(Vector3 pulseOrigin, float pulseForce, float pulseRadius);
}
