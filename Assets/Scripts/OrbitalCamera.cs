using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;

    public float rotationSpeed = 2.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;
    public float zoomSpeed = 2.0f;
    
    public float minYAngle = 5.0f;
    public float maxYAngle = 80.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 direction = transform.position - target.position;
        distance = direction.magnitude;
        Vector3 angles = Quaternion.LookRotation(direction).eulerAngles;
        currentX = angles.y;
        currentY = angles.x; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Error: No target assigned to OrbitalCamera");
            return;
        }
        
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0.0f);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;
        
        transform.position = position;
        transform.rotation = rotation;
    }
}
