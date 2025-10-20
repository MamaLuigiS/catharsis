using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothing = 0.125f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned");
            return;
        }
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smoothing);
        transform.position = smoothedPosition;
        
        transform.LookAt(target);
    }
}
