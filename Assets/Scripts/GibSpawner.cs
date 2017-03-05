using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class GibSpawner : MonoBehaviour
{
		/*
			* The object pool to take game objects from
			*/
		private ObjectPooler objPooler;
		// Use this for initialization
		void Start()
		{
				objPooler = GetComponent<ObjectPooler>();
		}

		public void SpawnGibs()
		{
				List<GameObject> gibs = objPooler.GetAllObjects();
				if (gibs.Count == 0)
				{
						return;
				}

				foreach (GameObject gib in gibs)
				{
						gib.transform.position = transform.position;
						gib.transform.rotation = transform.rotation;
						//spawn the enemy
						gib.SetActive(true);
				}
		}

		public void RemoveGibs()
		{
				List<GameObject> gibs = objPooler.GetAllObjects();
				if (gibs.Count == 0)
				{
						return;
				}

				foreach (GameObject gib in gibs)
				{
						//spawn the enemy
						gib.SetActive(false);
				}
		}
}
