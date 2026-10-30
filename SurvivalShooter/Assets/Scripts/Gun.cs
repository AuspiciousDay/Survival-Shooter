using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fireTransform;
    public ParticleSystem gunParticle;
    private LineRenderer lineRenderer;
    private AudioSource gunAudio;
    public PlayerInput playerInput;
    private float fireDistance = 50f;
    private float lastFireTime;
    public float damage = 10f;
    public GunData gunData;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        lastFireTime = 0f;
    }

    public void Fire()
    {
        if (Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    public LayerMask targetLayer;

    private void Shoot()
    {
        Vector3 hitPosition;

        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer))
        {
            hitPosition = hit.point;
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(CoShootEffect(hitPosition));
    }

    private IEnumerator CoShootEffect(Vector3 hitPosition)
    {
        gunParticle.Play();
        gunAudio.PlayOneShot(gunData.shotClip);

        lineRenderer.SetPosition(0, fireTransform.position);
        lineRenderer.SetPosition(1, hitPosition);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;
    }
}
