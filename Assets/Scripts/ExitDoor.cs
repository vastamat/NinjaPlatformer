using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class ExitDoor : MonoBehaviour
{
		public Sprite exitOpened;

		private SpriteRenderer sr;
		// Use this for initialization
		void Start()
		{
				sr = GetComponent<SpriteRenderer>();
		}

		void OnTriggerEnter2D(Collider2D coll)
		{
				if (coll.CompareTag("Player"))
				{
						sr.sprite = exitOpened;
						Debug.Log("Exit Reached, swap scene or whatever");
						SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}
		}
}
