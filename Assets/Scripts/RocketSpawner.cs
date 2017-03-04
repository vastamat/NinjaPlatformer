using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
		public float spawnRate;
		public GameObject rocket;
		// Use this for initialization
		void Start()
		{
				StartLaunching(1.0f);
		}
		void Update()
		{
				if (Input.GetKey(KeyCode.Alpha1))
				{
						StopLaunching();
				}
				if (Input.GetKey(KeyCode.Alpha2))
				{
						StartLaunching(1.0f);
				}
		}
		void LaunchRocket()
		{
				Instantiate(rocket, transform.position, transform.rotation);
		}

		void StartLaunching(float _secondsToFirstLaunch)
		{
				InvokeRepeating("LaunchRocket", _secondsToFirstLaunch, spawnRate);
		}
		void StopLaunching()
		{
				CancelInvoke("LaunchRocket");
		}
}
