using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class RocketSpawner : MonoBehaviour
{
		/*
			* The object pool to take game objects from
			*/
		private ObjectPooler objPooler;

		// seconds between the spawn of a new enemy
		public float spawnWait;

		// seconds to pass before the spawn the first wave of the game
		public float startWait;

		// Use this for initialization
		void Start()
		{
				objPooler = GetComponent<ObjectPooler>();

				StartLaunching(startWait);
		}
		void Update()
		{
		}
		void LaunchRocket()
		{
				//get a random enemy from the pool
				GameObject enemy = objPooler.GetNextFreeObject();

				//check if a free enemy was found
				if (enemy == null)
				{
						return;
				}
				enemy.transform.position = transform.position;
				enemy.transform.rotation = transform.rotation;

				//spawn the enemy
				enemy.SetActive(true);
		}

		void StartLaunching(float _secondsToFirstLaunch)
		{
				InvokeRepeating("LaunchRocket", _secondsToFirstLaunch, spawnWait);
		}

		void StopLaunching()
		{
				CancelInvoke("LaunchRocket");
		}

		void RemoveRockets()
		{
				List<GameObject> rockets = objPooler.GetAllObjects();
				if (rockets.Count == 0)
				{
						return;
				}

				foreach (GameObject rocket in rockets)
				{
						//spawn the enemy
						rocket.SetActive(false);
				}
		}
}
