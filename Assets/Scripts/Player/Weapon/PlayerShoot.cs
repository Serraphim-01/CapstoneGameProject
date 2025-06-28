using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Aiming")]
    public float maxAimDistance = 100f;   // How far the raycast can check
    public LayerMask aimMask;             // What layers the aim ray can hit

    [Header("Hit Feedback")]
    public GameObject hitEffectPrefab;    // Optional hit VFX
    public float hitForce = 500f;         // Force to apply to hit rigidbodies

[Header("Audio")]
public AudioClip gunShotSound;
private AudioSource audioSource;

    private PlayerAim playerAim;
    private Animator animator;
    private int aimLayerIndex;

    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        animator = GetComponentInChildren<Animator>();
        aimLayerIndex = animator.GetLayerIndex("Aim");

        audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.playOnAwake = false;
    }
    void Update()
    {
        HandleAiming();

        // Left click only fires if aiming with Right Mouse Button
        if (Input.GetMouseButtonDown(0) && playerAim.IsAiming)
        {
            animator.SetBool("IsShooting", true);
            ShootRaycast();
            Invoke(nameof(ResetIsShooting), 0.1f);
        }
    }
    void ResetIsShooting()
    {
        animator.SetBool("IsShooting", false);
    }
    void HandleAiming()
    {
        if (playerAim.IsAiming)
        {
            animator.SetBool("IsAiming", true);
            animator.SetLayerWeight(aimLayerIndex, 1f);
        }
        else
        {
            animator.SetBool("IsAiming", false);
            animator.SetLayerWeight(aimLayerIndex, 0f);
        }
    }

    void ShootRaycast()
    {
        if (gunShotSound != null && audioSource != null)
{
    audioSource.PlayOneShot(gunShotSound);
    audioSource.pitch = Random.Range(0.65f, 0.85f);
audioSource.PlayOneShot(gunShotSound);
audioSource.pitch = 1f; // Reset

}
        // 🔫 Fire from screen center with slight spread
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        // 🎯 Add spread by offsetting slightly in screen space
        float spreadRange = 1f; // max 1 unit spread
        screenCenter.x += Random.Range(-spreadRange, spreadRange);
        screenCenter.y += Random.Range(-spreadRange, spreadRange);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimMask))
        {
            // ✅ DAMAGE ENEMY if it has health script
            EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(25);
            }

            // ✅ Add force to physics objects
            if (hit.rigidbody != null)
            {
                Vector3 hitDirection = hit.point - ray.origin;
                hitDirection.Normalize();
                hit.rigidbody.AddForce(hitDirection * hitForce);
            }

            // ✅ Show hit effect
            if (hitEffectPrefab != null)
            {
                Quaternion rotation = Quaternion.LookRotation(-hit.normal); // face outward
                GameObject vfx = Instantiate(hitEffectPrefab, hit.point + hit.normal * 0.001f, rotation);

                // 🛑 Disable its colliders immediately so it won't block the next shot
                Collider[] colliders = vfx.GetComponentsInChildren<Collider>();
                foreach (Collider col in colliders)
                {
                    col.enabled = false;
                }

                // ⏳ Destroy it after a short time
                Destroy(vfx, 0.5f);
            }
        }
    }
}
