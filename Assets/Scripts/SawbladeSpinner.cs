using UnityEngine;

public class SawbladeSpinner : MonoBehaviour
{
		// Update is called once per frame
		void Update()
		{
				transform.Rotate(0.0f, 0.0f, 10.0f);
		}

		void OnCollisionEnter2D(Collision2D coll)
		{
				if (coll.gameObject.CompareTag("Player"))
				{
						Destroy(coll.gameObject);
				}
		}
}
