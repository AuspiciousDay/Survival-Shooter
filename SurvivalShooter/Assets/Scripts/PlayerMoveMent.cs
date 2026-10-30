using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    public static readonly int HashVertical = Animator.StringToHash("VerticalAixs");
    public static readonly int HashHorizontal = Animator.StringToHash("HorizontalAixs");

    public float moveSpeed = 5f;

    private Animator playerAnimator;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }
        if (direction.x == 0f && direction.z == 0f)
        {
            playerAnimator.SetBool("Move", false);
        }
        else
        {
            playerAnimator.SetBool("Move", true);
        }
        playerRigidbody.linearVelocity = direction * moveSpeed;
    }
}
