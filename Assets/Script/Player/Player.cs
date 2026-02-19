using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed  = 8f;
    [SerializeField] private float accelAmount = 12f;
    [SerializeField] private float decelAmount = 9f;
    [SerializeField] private Light2D flashlight;
    [SerializeField] private PolygonCollider2D flashlightCollider;
    [SerializeField] private Light2D flashlightSelfLight;

    private PlayerInputHandler input;
    private Rigidbody2D rb;
    private Animator anim;

    private PlayerState currentState;
    private bool footstepPlaying = false;

    void Start()
    {
        currentState = PlayerState.Walking;
        input = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        input.OnToggleFlashlight += ToggleFlashlight;
        input.OnDropItem += DropItem;
    }

    void FixedUpdate()
    {
        ApplyMovement(input.MoveInputNormalized, transform, rb);
        ApplyRotation(input.MousePosition, transform);

        anim.SetFloat("Speed", input.MoveInputNormalized.magnitude);
        currentState = input.MoveInputNormalized.magnitude > 0.01f ? PlayerState.Walking : PlayerState.Idle;


        if (currentState == PlayerState.Walking)
        {
            if (footstepPlaying) return;
            SoundManager.Instance.PlaySFXOn2nd(SoundManager.Footstep3, transform.position, loop: true);
            footstepPlaying = true;
        } else {
            if (!footstepPlaying) return;
            SoundManager.Instance.StopSFXOn2nd();
            footstepPlaying = false;
        }
    }

    public void ApplyMovement(Vector3 input, Transform transform, Rigidbody2D rb)
    {
        if (input.magnitude > 0.01f) // accelerate
        {
            Vector3 targetSpeed = new Vector3(input.x * moveSpeed, input.y * moveSpeed, 0f);

            Vector3 accelRate = new Vector3
            {
                x = (Mathf.Abs(targetSpeed.x) > 0.01f) ? accelAmount : decelAmount,
                y = (Mathf.Abs(targetSpeed.y) > 0.01f) ? accelAmount : decelAmount,
                z = 0f
            };

            //conserve momentum
            if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(targetSpeed.x) && Mathf.Sign(rb.linearVelocity.x) == Mathf.Sign(targetSpeed.x) && Mathf.Abs(targetSpeed.x) > 0.01f)
            {
                accelRate.x = 0;
            }
            if (Mathf.Abs(rb.linearVelocity.y) > Mathf.Abs(targetSpeed.y) && Mathf.Sign(rb.linearVelocity.y) == Mathf.Sign(targetSpeed.y) && Mathf.Abs(targetSpeed.y) > 0.01f)
            {
                accelRate.y = 0;
            }

            Vector3 speedDiff = new Vector3(targetSpeed.x - rb.linearVelocity.x, targetSpeed.y - rb.linearVelocity.y, 0f);
            Vector3 movement = new Vector3(speedDiff.x * accelRate.x, speedDiff.y * accelRate.y, 0f);

            rb.AddForce(movement.x * Vector3.right, ForceMode2D.Force);
            rb.AddForce(movement.y * Vector3.up, ForceMode2D.Force);
        } 
        else // decelerate 
        { 
            Vector3 movement = new Vector3(-rb.linearVelocity.x * decelAmount, -rb.linearVelocity.y * decelAmount, 0f);
            rb.AddForce(movement.x * Vector3.right, ForceMode2D.Force);
            rb.AddForce(movement.y * Vector3.up, ForceMode2D.Force);
        }
    }

    public void ApplyRotation(Vector3 targetPosition, Transform transform)
    {
        Vector2 dir = targetPosition - transform.position;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public void ToggleFlashlight()
    {
        flashlight.enabled = !flashlight.enabled;
        flashlightCollider.enabled = !flashlightCollider.enabled;
        flashlightSelfLight.enabled = !flashlightSelfLight.enabled;
        if (flashlight.enabled){
            SoundManager.Instance.PlaySFXAtPosition(SoundManager.FlashlightOn, transform.position);
        } else {
            SoundManager.Instance.PlaySFXAtPosition(SoundManager.FlashlightOn, transform.position, pitchOverride: 0.8f);
        }
    }

    public void DropItem()
    {
        InventoryManager.Instance.DropItem();
    }

    private enum PlayerState
    {
        Idle,
        Walking
    }
}

