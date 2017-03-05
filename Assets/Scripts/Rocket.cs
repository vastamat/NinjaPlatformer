using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{
		/** movement speed of the rocket */
		public float speed = 5.0f;

		/** rotation speed of the rocket */
		public float turnSpeed = 200.0f;

		/** flag whether it should collide with other geometry */
		public bool collideWithGeometry = true;

		/** Reference to the player's transform to follow him */
		private Transform playerTransform;
		/** Reference to the this objects rigidbody */
		private Rigidbody2D rb;
		// Use this for initialization
		void Start()
		{
				//get the rigidbody reference
				rb = GetComponent<Rigidbody2D>();
				//get the player's transform
				playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		}

		// Update is called once per frame
		void FixedUpdate()
		{
				//calculate the direction vector to the player
				Vector2 point2Target = (Vector2)transform.position - (Vector2)playerTransform.position;

				//normalize it
				point2Target.Normalize();

				//get the z rotation value from the axis perpendicular of the x and y axis
				float value = Vector3.Cross(point2Target, transform.up).z;

				//set the rockets angular velocity for rotation
				rb.angularVelocity = turnSpeed * value;

				//set the rockets velocity for movement
				rb.velocity = transform.up * speed;
		}

		void OnTriggerEnter2D(Collider2D coll)
		{
				if (!coll.CompareTag("Player"))
				{
						if (collideWithGeometry)
						{
								Obliterate();
						}
				}
		}

		void Obliterate()
		{
				gameObject.SetActive(false);
		}
}
