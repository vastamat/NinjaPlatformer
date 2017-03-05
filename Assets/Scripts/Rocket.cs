using UnityEngine;

public class Rocket : MonoBehaviour
{
		public float speed = 5.0f;
		public float turnSpeed = 200.0f;
		public bool collideWithGeometry = true;

		private Transform playerTransform;
		private Rigidbody2D rb;
		// Use this for initialization
		void Start()
		{
				rb = GetComponent<Rigidbody2D>();
				playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		}

		// Update is called once per frame
		void FixedUpdate()
		{
				Vector2 point2Target = (Vector2)transform.position - (Vector2)playerTransform.position;

				point2Target.Normalize();

				float value = Vector3.Cross(point2Target, transform.up).z;

				rb.angularVelocity = turnSpeed * value;

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
