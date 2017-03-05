using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
		//The transform of the player
		private Transform playerTrans;
		// Use this for initialization
		void Start()
		{
				//find the player and store his transform
				playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
		}

		// Update is called once per frame
		void Update()
		{
				//direction vector from this object to the player
				Vector3 diff = (playerTrans.position - transform.position).normalized;

				//calculate the z rotation of the direction vector
				float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

				//set the orientation to the rotZ - 90 for correct results
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90.0f);
		}
}
