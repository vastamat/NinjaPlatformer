using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
		private float moveSpeed = 6.0f;
		private float gravity = -20;
		private Vector2 velocity;

		private PlayerController controller;

		// Use this for initialization
		void Start()
		{
				controller = GetComponent<PlayerController>();
		}

		// Update is called once per frame
		void Update()
		{
				Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

				velocity.x = input.x * moveSpeed;
				//apply gravity
				velocity.y += gravity * Time.deltaTime;
				controller.Move(velocity * Time.deltaTime);
		}
}
