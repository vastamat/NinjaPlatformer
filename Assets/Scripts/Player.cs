using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
		public float jumpHeight = 4;
		public float timeToHighestPoint = 0.4f;
		public float moveSpeed = 6.0f;
		public float accelerationTimeInAir = 0.2f;
		public float accelerationTimeOnGround = 0.1f;
		public float maxWallSlideSpeed = 3.0f;
		public float wallStickTime = 0.25f;
		public Vector2 wallJumpClimb;
		public Vector2 wallJumpOff;
		public Vector2 wallLeap;

		private float timeToWallUnstick;
		private float jumpVelocity;
		private float gravity;
		private float velocityXSmooth;

		private Vector2 velocity;

		private PlayerController controller;

		// Use this for initialization
		void Start()
		{
				controller = GetComponent<PlayerController>();

				gravity = -(2 * jumpHeight) / Mathf.Pow(timeToHighestPoint, 2);
				jumpVelocity = Mathf.Abs(gravity) * timeToHighestPoint;
		}

		// Update is called once per frame
		void Update()
		{
				PlayerController.CollisionInfo collisions = controller.GetCollisionInfo();

				Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

				int wallDirX = (collisions.left) ? -1 : 1;

				float targetVelocity = input.x * moveSpeed;

				velocity.x = Mathf.SmoothDamp(velocity.x,
						targetVelocity,
						ref velocityXSmooth,
						collisions.below ? accelerationTimeOnGround : accelerationTimeInAir);

				bool wallSliding = false;


				if ((collisions.left || collisions.right) && !collisions.below && velocity.y < 0.0f)
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

								if (input.x != wallDirX && input.x != 0.0f)
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

				if (collisions.above || collisions.below)
				{
						velocity.y = 0.0f;
				}


				if (Input.GetKeyDown(KeyCode.Space))
				{
						if (wallSliding)
						{
								if (wallDirX == input.x)
								{
										velocity.x = -wallDirX * wallJumpClimb.x;
										velocity.y = wallJumpClimb.y;
								}
								else if (input.x == 0.0f)
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
						if (collisions.below)
						{
								velocity.y = jumpVelocity;
						}
				}

				//apply gravity
				velocity.y += gravity * Time.deltaTime;
				controller.Move(velocity * Time.deltaTime);
		}
}
