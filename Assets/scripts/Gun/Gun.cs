using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Shooting Settings")]
    public float fireRate = 0.5f;
    public AudioClip shootSound;

    private AudioSource audioSource;
    private float nextTimeToFire = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.85f;
        audioSource.pitch = 1.0f;
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // ONLY shoot if the player is currently holding this gun
        if (PlayerHand.currentHeldObject != gameObject)
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Fire bullet
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Play shooting sound
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}