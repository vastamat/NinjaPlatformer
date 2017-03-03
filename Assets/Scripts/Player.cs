using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{
		private PlayerController controller;

		// Use this for initialization
		void Start()
		{
				controller = GetComponent<PlayerController>();
		}

		// Update is called once per frame
		void Update()
		{

		}
}
