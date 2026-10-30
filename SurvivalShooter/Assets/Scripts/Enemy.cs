using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die
    }
    public Transform target;
    public LayerMask targetLayer;

    public float traceDistance = 25f;
    public float attackDistance = 1.2f;

    public ParticleSystem particle;
    public Collider EnemyCollider;
    public AudioClip hitClip;
    public AudioClip deathClip;

    private NavMeshAgent agent;
    private Animator EnemyAnimator;
    private AudioSource EnemyAudioSource;

    private float damage;
    private float lastAttackTime;
    public float attackInterval = 1f;
    private Status currentStatus;

    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
    public static readonly int IdDie = Animator.StringToHash("Die");

    private float sinkSpeed = 1f;
    private float sinkDuration = 2f;
    private bool isSinking = false;
    private float sinkTimer = 0f;

    public int score = 50;

    public  EnemyData enemyData;

    private void Start()
    {
        Setup(enemyData);
    }
    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;
            switch (currentStatus)
            {
                case Status.Idle:
                    EnemyAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    EnemyAnimator.SetBool(IdHasTarget, true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    EnemyAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    EnemyAnimator.SetTrigger(IdDie);
                    agent.isStopped = true;
                    EnemyCollider.enabled = false;
                    EnemyAudioSource.PlayOneShot(deathClip);
                    break;
            }
        }
    }
    public void Setup(EnemyData data)
    {
        startingHealth = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyAudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        isSinking = false;
        sinkTimer = 0f;
        target = null;
        lastAttackTime = 0f;

        agent.enabled = true;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            CurrentStatus = Status.Idle;
        }
        else
        {
            agent.enabled = false;
        }

        EnemyCollider.enabled = true;
    }

    private void OnDisable()
    {
        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }
    private void UpdateDie()
    {
        if (!isSinking)
        {
            return;
        }
        transform.position += Vector3.down * sinkSpeed * Time.deltaTime;

        sinkTimer += Time.deltaTime;

        if(sinkTimer >= sinkDuration)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateAttack()
    {
        if (target == null || Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        if (Time.time >= lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                var hitNormal = (transform.position - target.position).normalized;
                damageable.OnDamage(damage, transform.position, hitNormal);
            }
        }
    }

    private void UpdateTrace()
    {
        if (target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }
        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        agent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        Transform closet = null;
        float closetDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closetDistance)
            {
                closetDistance = distance;
                closet = colliders[i].transform;
            }
        }
        return closet;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDeath)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitNormal);

        EnemyAudioSource.PlayOneShot(hitClip);
        particle.transform.position = hitPoint;
        particle.transform.forward = hitNormal;
        particle.Play();
    }

    public override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;
    }
    public void StartSinking()
    {
        if (isSinking)
        {
            return;
        }
        isSinking = true;
        sinkTimer = 0f;
        if(agent != null && agent.isActiveAndEnabled)
        {
            agent.enabled =false;
        }
    }

}
