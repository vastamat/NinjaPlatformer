using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
		private Transform playerTrans;
		// Use this for initialization
		void Start()
		{
				playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
		}

		// Update is called once per frame
		void Update()
		{
				Vector3 diff = (playerTrans.position - transform.position).normalized;

				float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90.0f);
		}
}
