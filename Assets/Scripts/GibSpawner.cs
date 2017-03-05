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
				// get the object pooler component
				objPooler = GetComponent<ObjectPooler>();
		}

		public void SpawnGibs()
		{
				//get all objects
				List<GameObject> gibs = objPooler.GetAllObjects();
				if (gibs.Count == 0)
				{
						//quietly return if there are no pooled objects
						return;
				}
				//"spawn" all objects at this objects' transform
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
				//get all objects
				List<GameObject> gibs = objPooler.GetAllObjects();
				if (gibs.Count == 0)
				{
						//return if there are none
						return;
				}

				foreach (GameObject gib in gibs)
				{
						//deactivate them all
						gib.SetActive(false);
				}
		}
}
