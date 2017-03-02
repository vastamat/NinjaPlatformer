using UnityEngine;

public class PlayerController : MonoBehaviour
{
		public float movementSpeed = 500.0f;
		public float jumpForce = 100.0f;
		public float maxVelocity = 10.0f;

		private Rigidbody2D rb;
		private Animator animator;
		private SpriteRenderer sr;
		private bool isJumping = false;

		// Use this for initialization
		void Start()
		{
				rb = GetComponent<Rigidbody2D>();
				animator = GetComponent<Animator>();
				sr = GetComponent<SpriteRenderer>();
		}

		void FixedUpdate()
		{
				float horizontal = Input.GetAxis("Horizontal");

				rb.AddForce(Vector2.right * horizontal * movementSpeed);

				if (!isJumping)
				{
						if (Input.GetKey(KeyCode.Space))
						{
								//rb.velocity = new Vector2(rb.velocity.x, 0.0f);
								rb.AddForce(Vector2.up * jumpForce);
								isJumping = true;
						}
				}

				//clamp horizontal velocity to max
				rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity), rb.velocity.y);

				if (rb.velocity.x < 0.0f)
				{
						sr.flipX = true;
				}
				else if (rb.velocity.x > 0.0f)
				{
						sr.flipX = false;
				}

				animator.SetFloat("velocity", rb.velocity.x);
		}

		void OnCollisionEnter2D(Collision2D _collision)
		{
				if (_collision.gameObject.CompareTag("Jumpable"))
				{
						isJumping = false;
				}
		}
}
