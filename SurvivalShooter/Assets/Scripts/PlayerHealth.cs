using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public readonly static int IdDie = Animator.StringToHash("Die");

    public Image uiHealth;
    public Image uiMaxHealth;

    public AudioClip hitClip;
    public AudioClip deathClip;

    private AudioSource playerAudioSource;
    private Animator playerAnimator;

    private PlayerMoveMent playerMoveMent;
    private PlayerInput playerInput;
    private PlayerRotation playerRotation;

    public RectTransform rectTransform;
    private float maxWidth = 275f;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        playerMoveMent = GetComponent<PlayerMoveMent>();
        playerInput = GetComponent<PlayerInput>();
        playerRotation = GetComponent<PlayerRotation>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        playerMoveMent.enabled = true;
        playerInput.enabled = true;
        playerRotation.enabled = true;

        uiHealth.gameObject.SetActive(true);
        uiMaxHealth.gameObject.SetActive(true);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDeath)
        {
            playerAudioSource.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
        UpdateHealth(Health, startingHealth);
    }
    public void UpdateHealth(float currentHp, float maxHp)
    {
        float ratio = Mathf.Clamp01(currentHp / maxHp);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth * ratio);
    }
    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger(IdDie);
        playerAudioSource.PlayOneShot(deathClip);

        playerMoveMent.enabled = false;
        playerInput.enabled = false;
        playerRotation.enabled = false;

    }
}
