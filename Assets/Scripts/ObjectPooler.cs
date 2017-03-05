using UnityEngine;
using System.Collections.Generic;

/*
 * Script to pool objects of type objectToSpawn and return free(inactive) objects
	* The pooled objects are stored as children to the object this script is attached to
 */
public class ObjectPooler : MonoBehaviour
{
		// Array of gameobjects to pool
		public GameObject[] gameObjects;
		// The ammout of instances to pool
		public int pooledAmount = 10;
		// Flag whether the pool can expand or not
		public bool canExpand = false;
		// The max size of the pool
		public int maxSize = 20;

		//the pooled objects storage
		private List<GameObject> pooledObjects;

		/*
		 * Initialize the object pool by instantiating all the objects
			*  as children to this gameObject and setting them to inactive
		 */
		void Start()
		{
				//Create the list of game objects
				pooledObjects = new List<GameObject>();

				// Instantiate and pool the needed pool amount
				for (int i = 0; i < pooledAmount; i++)
				{
						// Instantiate by circling from all the game objects
						GameObject obj = Instantiate(gameObjects[i % gameObjects.Length]);
						// set it to inactive 
						obj.SetActive(false);
						// add it to the list
						pooledObjects.Add(obj);
				}
		}

		/*
		 * Returns the next free game object in the list
			* if all of them are active, then the storage can potentially expand if the setting allow it
			* otherwise returns null
		 */
		public GameObject GetNextFreeObject()
		{
				for (int i = 0; i < pooledObjects.Count; i++)
				{
						// Check if the object is inactive
						if (!pooledObjects[i].activeInHierarchy)
						{
								// if it's inactive, return it as it's free
								return pooledObjects[i];
						}
				}

				//if there are no free objects, check if the pool can expand
				if (canExpand && pooledObjects.Count < maxSize)
				{
						//if the pool can expand, create a new game object from a randomly selected object from the array
						GameObject newObj = Instantiate(gameObjects[Random.Range(0, gameObjects.Length)]);
						// set it to inactive
						newObj.SetActive(false);
						// add it to the pool
						pooledObjects.Add(newObj);
						// return it as it's a new free object
						return newObj;
				}
				// if no object is available, return null
				return null;
		}

		public GameObject GetObjectAt(int index)
		{
				//if the index is not in the correct range, return null
				if (index < 0 || index > pooledObjects.Count)
				{
						return null;
				}
				//check if the object at this index is free (inactive)
				if (!pooledObjects[index].activeInHierarchy)
				{
						return pooledObjects[index];
				}
				//if it's not free, return null
				return null;
		}

		public GameObject GetRandomObject()
		{
				// get a random index from 0 to the count of pooled objects
				int randomIndex = Random.Range(0, pooledObjects.Count);
				// check if the object at the random index is free
				if (!pooledObjects[randomIndex].activeInHierarchy)
				{
						//if it is, return it
						return pooledObjects[randomIndex];
				}
				//if not, just try to return a free object
				return GetNextFreeObject();
		}

		public List<GameObject> GetAllObjects()
		{
				return pooledObjects;
		}
}
