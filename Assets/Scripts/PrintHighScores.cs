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
						float highscore = GlobalControl.instance.savedStats.highScores[i];
						if (highscore != 0.0f)
						{
								int minutes = (int)(highscore / 60.0f);
								int seconds = (int)Mathf.Repeat(highscore, 60.0f);
								int millis = (int)Mathf.Repeat(highscore * 1000.0f, 1000.0f);
								string scoreText = minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + "." + millis.ToString().PadLeft(3, '0');
								highscoreTexts[i].text = "#" + (i + 1) + ": " + scoreText;
						}
						else
						{
								highscoreTexts[i].text = "#" + (i + 1) + ": Empty";
						}
				}
		}
}
