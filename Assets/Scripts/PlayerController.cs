using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
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
		}

		void Update()
		{
				UpdateRaycastOrigins();
				CalculateRaySpacing();

				for (int i = 0; i < verticalRayCount; i++)
				{
						Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);
				}
		}

		void UpdateRaycastOrigins()
		{
				Bounds bounds = GetShrunkenBounds();

				raycastOrigins.bottomLeft  = new Vector2(bounds.min.x, bounds.min.y);
				raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
				raycastOrigins.topLeft				 = new Vector2(bounds.min.x, bounds.max.y);
				raycastOrigins.topRight			 = new Vector2(bounds.max.x, bounds.max.y);

		}

		void CalculateRaySpacing()
		{
				Bounds bounds = GetShrunkenBounds();

				horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
				verticalRayCount		 = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

				horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
				verticalRaySpacing		 = bounds.size.x / (verticalRayCount - 1);
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
