using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AnimateBackground : MonoBehaviour
{
		public float speed = 1.0f;
		public Texture2D[] images;

		private ScreenOverlay overlay;
		private float counter = 0.0f;
		private int currIndex = 0;

		// Use this for initialization
		void Start()
		{
				overlay = GetComponent<ScreenOverlay>();
		}

		// Update is called once per frame
		void Update()
		{
				counter += speed * Time.deltaTime;

				if (counter >= 1.0f)
				{
						counter = 0.0f;
						currIndex++;
						if (currIndex >= images.Length) currIndex = 0;
						overlay.texture = images[currIndex];
				}
		}
}
