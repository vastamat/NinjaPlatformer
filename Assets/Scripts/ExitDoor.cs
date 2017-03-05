using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class ExitDoor : MonoBehaviour
{
		/** The sprite to use  when the player reaches the exit */
		public Sprite exitOpened;

		/** The sprite renderer of the player */
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
						SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}
		}
}
