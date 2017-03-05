using UnityEngine;
using UnityEngine.UI;

public class PrintHighScores : MonoBehaviour
{
		//The array of texts for the highscores
		private Text[] highscoreTexts;

		void Start()
		{
				// Get all the text's
				highscoreTexts = GetComponentsInChildren<Text>();

				// Set the text for the highscores
				for (int i = 0; i < highscoreTexts.Length; i++)
				{
						highscoreTexts[i].text = "#" + (i + 1) + ": " + GlobalControl.instance.savedStats.highScores[i];
				}
		}
}
