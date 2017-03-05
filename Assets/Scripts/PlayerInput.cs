using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
		/** Reference to the player to call his public functions */
		private Player player;
		// Use this for initialization
		void Start()
		{
				//get the player component
				player = GetComponent<Player>();
		}

		// Update is called once per frame
		void Update()
		{
				//get the directional input
				Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

				//send the input to the player
				player.SetDirectionalInput(directionalInput);

				//handle jump inputs
				if (Input.GetKeyDown(KeyCode.Space))
				{
						player.OnJumpInputDown();
				}
				if (Input.GetKeyUp(KeyCode.Space))
				{
						player.OnJumpInputUp();
				}
		}
}
