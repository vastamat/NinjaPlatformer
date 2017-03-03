using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
		public LayerMask collisionMask;
		public int horizontalRayCount = 4;
		public int verticalRayCount = 4;

		private float horizontalRaySpacing;
		private float verticalRaySpacing;

		private const float skinWidth = 0.015f;
		private BoxCollider2D capsuleCol;
		private RaycastOrigins raycastOrigins;

		void Start()
		{
				capsuleCol = GetComponent<BoxCollider2D>();
				CalculateRaySpacing();
		}

		public void Move(Vector2 _velocity)
		{
				UpdateRaycastOrigins();

				if (_velocity.x != 0.0f)
				{
						HorizontalCollisions(ref _velocity);
				}
				if (_velocity.y != 0.0f)
				{
						VerticalCollisions(ref _velocity);
				}

				transform.Translate(_velocity);
		}

		void HorizontalCollisions(ref Vector2 _velocity)
		{
				float directionX = Mathf.Sign(_velocity.x);
				float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

				for (int i = 0; i < horizontalRayCount; i++)
				{
						Vector2 rayOrigin = (directionX == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
						rayOrigin += Vector2.up * (horizontalRaySpacing * i);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

						if (hit)
						{
								_velocity.x = (hit.distance - skinWidth) * directionX;
								rayLength = hit.distance;
						}
				}
		}

		void VerticalCollisions(ref Vector2 _velocity)
		{
				float directionY = Mathf.Sign(_velocity.y);
				float rayLength = Mathf.Abs(_velocity.y) + skinWidth;

				for (int i = 0; i < verticalRayCount; i++)
				{
						Vector2 rayOrigin = (directionY == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.topLeft;
						rayOrigin += Vector2.right * (verticalRaySpacing * i + _velocity.x);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

						if (hit)
						{
								_velocity.y = (hit.distance - skinWidth) * directionY;
								rayLength = hit.distance;
						}
				}
		}

		void UpdateRaycastOrigins()
		{
				Bounds bounds = GetShrunkenBounds();

				raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
				raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
				raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
				raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

		}

		void CalculateRaySpacing()
		{
				Bounds bounds = GetShrunkenBounds();

				horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
				verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

				horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
				verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}

		Bounds GetShrunkenBounds()
		{
				Bounds bounds = capsuleCol.bounds;

				bounds.Expand(skinWidth * -2);

				return bounds;
		}

		struct RaycastOrigins
		{
				public Vector2 topLeft, topRight;
				public Vector2 bottomLeft, bottomRight;
		}


		//public float movementSpeed = 500.0f;
		//public float jumpForce = 100.0f;
		//public float maxVelocity = 10.0f;

		//private Rigidbody2D rb;
		//private Animator animator;
		//private SpriteRenderer sr;
		//private bool isJumping = false;

		//// Use this for initialization
		//void Start()
		//{
		//		rb = GetComponent<Rigidbody2D>();
		//		animator = GetComponent<Animator>();
		//		sr = GetComponent<SpriteRenderer>();
		//}

		//void FixedUpdate()
		//{
		//		float horizontal = Input.GetAxis("Horizontal");

		//		rb.AddForce(Vector2.right * horizontal * movementSpeed);

		//		if (!isJumping)
		//		{
		//				if (Input.GetKey(KeyCode.Space))
		//				{
		//						//rb.velocity = new Vector2(rb.velocity.x, 0.0f);
		//						rb.AddForce(Vector2.up * jumpForce);
		//						isJumping = true;
		//				}
		//		}

		//		//clamp horizontal velocity to max
		//		rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity), rb.velocity.y);

		//		if (rb.velocity.x < 0.0f)
		//		{
		//				sr.flipX = true;
		//		}
		//		else if (rb.velocity.x > 0.0f)
		//		{
		//				sr.flipX = false;
		//		}

		//		animator.SetFloat("velocity", rb.velocity.x);
		//}

		//void OnCollisionEnter2D(Collision2D _collision)
		//{
		//		if (_collision.gameObject.CompareTag("Jumpable"))
		//		{
		//				isJumping = false;
		//		}
		//}
}
