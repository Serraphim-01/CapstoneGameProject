using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float distance = 5f;
    public float sensitivity = 5f;
    public float minY = -30f;
    public float maxY = 70f;

    private float currentYaw;
    private float currentPitch;

    public bool isAiming { get; set; }

    void LateUpdate()
    {
        if (target == null) return;

        // Free movement / aim logic
        currentYaw += Input.GetAxis("Mouse X") * sensitivity;
        currentPitch -= Input.GetAxis("Mouse Y") * sensitivity;
        currentPitch = Mathf.Clamp(currentPitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 aimDirection = rotation * Vector3.back;
        Vector3 aimPosition = target.position + new Vector3(0, offset.y, 0) + aimDirection * distance;

        transform.position = aimPosition;
        transform.LookAt(target.position + new Vector3(0, offset.y, 0));
    }
}
