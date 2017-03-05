using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
		public Text text;
		// Update is called once per frame
		void Update()
		{
				int minutes = (int)(Time.time / 60.0f);
				int seconds = (int)Mathf.Repeat(Time.time, 60.0f);
				int millis = (int)Mathf.Repeat(Time.time * 1000.0f, 1000.0f);
				text.text = minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + "." + millis.ToString().PadLeft(3, '0');
		}
}
