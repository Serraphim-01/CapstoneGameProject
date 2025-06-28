using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float crouchSpeed = 1f;
    public float gravity = -9.81f;
    public float crouchHeight = 1f;
    public float normalHeight = 2f;
    public float rotationSmoothTime = 0.1f;
    public float speedSmoothTime = 0.1f;

    [Header("Key Bindings")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;
    private bool isCrouching;

    private float currentSpeed;
    private float speedVelocity;
    private float currentAngle;
    private float angleVelocity;

    private PlayerAim playerAim; // reference to the aiming script
    
    [Header("Joystick")]
    public FloatingJoystick joystick; 


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerAim = GetComponent<PlayerAim>(); // get the aim script

    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        HandleStates();

        Vector2 inputVec = joystick != null ? joystick.InputDirection : Vector2.zero;
Vector3 inputDirection = new Vector3(inputVec.x, 0f, inputVec.y).normalized;
;


        // 🔥 Camera-relative movement
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

Vector3 moveDirection = camForward * inputVec.y + camRight * inputVec.x;
        moveDirection.Normalize();

        if (!playerAim.IsAiming)
        {
            if (moveDirection.magnitude >= 0.1f)
            {
                // Normal rotation based on movement
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                currentAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angleVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            }
        }
        else
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f; // Don't tilt the player up/down
            cameraForward.Normalize();

            if (cameraForward != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }


        float maxSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        float targetSpeedRaw = maxSpeed * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeedRaw, ref speedVelocity, speedSmoothTime);

        Vector3 move = isCrouching ? Vector3.zero : moveDirection * currentSpeed;
        controller.Move(move * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animation update
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsCrouching", isCrouching);
    }


    void HandleStates()
    {
        isRunning = Input.GetKey(runKey) && !isCrouching;

        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? crouchHeight : normalHeight;
        }
    }
}
