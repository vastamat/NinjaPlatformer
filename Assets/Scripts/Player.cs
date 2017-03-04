using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToHighestPoint = 0.4f;

    public float moveSpeed = 6.0f;
    public float accelerationTimeInAir = 0.2f;
    public float accelerationTimeOnGround = 0.1f;

    public float maxWallSlideSpeed = 3.0f;
    public float wallStickTime = 0.25f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public GameObject GibPrefab;
    public AudioClip DeathSound;

    private float timeToWallUnstick;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float gravity;
    private float velocityXSmooth;

    private Vector2 velocity;

    private PlayerController controller;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 directionalInput;
    private bool wallSliding = false;
    private int wallDirX;

    private Vector3 startPos;
    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        controller = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToHighestPoint, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToHighestPoint;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime);

        if (controller.GetCollisionInfo().above || controller.GetCollisionInfo().below)
        {
            if (controller.GetCollisionInfo().slidingDownMaxSlope)
            {
                velocity.y += controller.GetCollisionInfo().slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0.0f;
            }
        }

        anim.SetFloat("velocityX", Mathf.Abs(velocity.x));
        anim.SetFloat("velocityY", Mathf.Abs(velocity.y));

        anim.SetBool("isSliding", controller.GetCollisionInfo().slidingDownMaxSlope || wallSliding);

        if (velocity.x > 0.0f)
        {
            sr.flipX = false;
        }
        else if (velocity.x < 0.0f)
        {
            sr.flipX = true;
        }

        if (controller.GetCollisionInfo().fatalCollision)
        {
            Die();
        }
    }

    public void SetDirectionalInput(Vector2 _input)
    {
        directionalInput = _input;
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0.0f)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.GetCollisionInfo().below)
        {
            if (controller.GetCollisionInfo().slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.GetCollisionInfo().slopeNormal.x))
                {
                    //not jumping against max slope
                    velocity.y = maxJumpVelocity * controller.GetCollisionInfo().slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.GetCollisionInfo().slopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x,
                targetVelocityX,
                ref velocityXSmooth,
                (controller.GetCollisionInfo().below) ? accelerationTimeOnGround : accelerationTimeInAir);
        velocity.y += gravity * Time.deltaTime;
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.GetCollisionInfo().left) ? -1 : 1;

        wallSliding = false;

        if ((controller.GetCollisionInfo().left || controller.GetCollisionInfo().right) && !controller.GetCollisionInfo().below && velocity.y < 0.0f)
        {
            wallSliding = true;

            if (velocity.y < -maxWallSlideSpeed)
            {
                velocity.y = -maxWallSlideSpeed;
            }

            if (timeToWallUnstick > 0.0f)
            {
                velocity.x = 0.0f;
                velocityXSmooth = 0.0f;

                if (directionalInput.x != wallDirX && directionalInput.x != 0.0f)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void Die()
    {

        GameObject[] gibs = GameObject.FindGameObjectsWithTag("Gore");
        foreach (var item in gibs)
        {
            GameObject.Destroy(item);
        }

        AudioSource.PlayClipAtPoint(DeathSound, this.transform.position);

        for (int i = 0; i < 4; i++)
        {
            GameObject tempGore = GameObject.Instantiate<GameObject>(GibPrefab);
            tempGore.transform.position = this.transform.position;
            tempGore.GetComponent<Rigidbody2D>().velocity = this.velocity;
        }

        velocity.x = 0.0f;
        velocity.y = 0.0f;
        transform.position = startPos;

    }
}
