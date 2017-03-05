using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AnimateBackground : MonoBehaviour
{
		/** Animation speed */
		public float speed = 1.0f;
		/** Images for animation */
		public Texture2D[] images;

		/** The screen overlay component */
		private ScreenOverlay overlay;
		/** counter for how long the animtion should stay */
		private float counter = 0.0f;
		/** current texture index used  */
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
