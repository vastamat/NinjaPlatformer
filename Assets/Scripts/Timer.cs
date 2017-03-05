using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
		public Text text;

		private float time = 0.0f;
		// Update is called once per frame
		void Update()
		{
				time += Time.deltaTime;
				int minutes = (int)(time / 60.0f);
				int seconds = (int)Mathf.Repeat(time, 60.0f);
				int millis = (int)Mathf.Repeat(time * 1000.0f, 1000.0f);
				text.text = minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + "." + millis.ToString().PadLeft(3, '0');
		}

		public void RefreshTimer()
		{
				time = 0.0f;
		}

		void OnDisable()
		{
				//When the user is exiting this level, save the time he survived
				GlobalControl.instance.timeSurvived = time;
		}
}
