using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
		public float jumpHeight = 4;
		public float timeToHighestPoint = 0.4f;
		public float moveSpeed = 6.0f;
		public float accelerationTimeInAir = 0.2f;
		public float accelerationTimeOnGround = 0.1f;
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

				print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
		}

		// Update is called once per frame
		void Update()
		{
				PlayerController.CollisionInfo collisions = controller.GetCollisionInfo();
				if (collisions.above || collisions.below)
				{
						velocity.y = 0.0f;
				}

				Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

				if (Input.GetKeyDown(KeyCode.Space) && collisions.below)
				{
						velocity.y = jumpVelocity;
				}

				float targetVelocity = input.x * moveSpeed;
				velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmooth, collisions.below ? accelerationTimeOnGround : accelerationTimeInAir);
				//apply gravity
				velocity.y += gravity * Time.deltaTime;
				controller.Move(velocity * Time.deltaTime);
		}
}
