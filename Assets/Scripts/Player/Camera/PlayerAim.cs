using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform cameraTransform;

    private Animator animator;
    private int aimLayerIndex;
    private ThirdPersonCamera thirdPersonCamera;

    public bool IsAiming { get; private set; }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        aimLayerIndex = animator.GetLayerIndex("Aim");
        thirdPersonCamera = Camera.main.GetComponent<ThirdPersonCamera>();

    }

    void Update()
{
    IsAiming = Input.GetMouseButton(1);

    // Disable third person camera orbit during aim
    if (thirdPersonCamera != null)
        thirdPersonCamera.isAiming = false;

    // Trigger aim animation
    animator.SetBool("IsAiming", IsAiming);
    animator.SetLayerWeight(aimLayerIndex, IsAiming ? 1f : 0f);

    // You can optionally rotate the player here or let PlayerController do it
}

}
