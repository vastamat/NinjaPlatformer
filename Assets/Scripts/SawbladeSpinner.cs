using UnityEngine;

public class SawbladeSpinner : MonoBehaviour
{
		/** How many degrees per sec should the saw spin at */
		public float degreePerSec = 10.0f;
		// Update is called once per frame
		void Update()
		{
				transform.Rotate(0.0f, 0.0f, degreePerSec);
		}
}
