using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(GibSpawner))]
public class Player : MonoBehaviour
{
		/** The maximum jump height in unity units */
		public float maxJumpHeight = 4;
		/** The minimum jump height in unity units */
		public float minJumpHeight = 1;
		/** The time it will take to reach the highest point of the jump */
		public float timeToHighestPoint = 0.4f;

		/** The player's move speed */
		public float moveSpeed = 6.0f;
		/** The player's acceleration in air */
		public float accelerationTimeInAir = 0.2f;
		/** The player's acceleration on ground */
		public float accelerationTimeOnGround = 0.1f;

		/** The maximum speed a player can have while sliding a wall */
		public float maxWallSlideSpeed = 3.0f;
		/** The time a player has after pressing opposite direction of a wall before he falls normally */
		public float wallStickTime = 0.25f;

		/** velocity vector of a player's jump while pressing the input towards the wall direction */
		public Vector2 wallJumpClimb;
		/** velocity vector of a player's jump while pressing no input */
		public Vector2 wallJumpOff;
		/** velocity vector of a player's jump while pressing the input opposite of the wall direction */
		public Vector2 wallLeap;

		/** Clip to play when the player dies */
		public AudioClip DeathSound;

		/** The time it takes to unstick from a wall */
		private float timeToWallUnstick;
		/** the maximum jump velocity */
		private float maxJumpVelocity;
		/** the minimum jump velocity */
		private float minJumpVelocity;
		/** the gravity affecting the player */
		private float gravity;
		/** the smoothing on the x axis for smooth movements */
		private float velocityXSmooth;

		/** the velocity of the player */
		private Vector2 velocity;

		/** the player controller which recieves input and moves him with collision detection */
		private PlayerController controller;
		/** the player animator */
		private Animator anim;
		/** the player sprite renderer */
		private SpriteRenderer sr;
		/** the gibspawner to spawn gibs when the player dies */
		private GibSpawner gs;

		/** the the directional input the player recieves */
		private Vector2 directionalInput;
		/** flag whether the player is currently sliding down a wall */
		private bool wallSliding = false;
		/** The direction of the wall the player is sliding (-1 for left, 1 for right) */
		private int wallDirX;

		/** The player's starting position, used for "respawning" */
		private Vector3 startPos;

		// Use this for initialization
		void Start()
		{
				startPos = transform.position;
				controller = GetComponent<PlayerController>();
				anim = GetComponent<Animator>();
				sr = GetComponent<SpriteRenderer>();
				gs = GetComponent<GibSpawner>();

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
								JumpOffWall(-wallDirX, wallJumpClimb);
						}
						else if (directionalInput.x == 0.0f)
						{
								JumpOffWall(-wallDirX, wallJumpOff);
						}
						else
						{
								JumpOffWall(-wallDirX, wallLeap);
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

		private void JumpOffWall(int _direction, Vector2 _jumpVelocity)
		{
				velocity.x = _direction * _jumpVelocity.x;
				velocity.y = _jumpVelocity.y;
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
				AudioSource.PlayClipAtPoint(DeathSound, transform.position);

				GameObject[] rockets = GameObject.FindGameObjectsWithTag("FatalRefreshable");
				foreach (GameObject rocket in rockets)
				{
						rocket.SetActive(false);
				}

				gs.RemoveGibs();
				gs.SpawnGibs();

				velocity.x = 0.0f;
				velocity.y = 0.0f;
				transform.position = startPos;
		}
}
