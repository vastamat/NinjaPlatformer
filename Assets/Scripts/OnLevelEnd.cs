using UnityEngine;
using UnityEngine.UI;

/*Script to be called on the level end scene*/
public class OnLevelEnd : MonoBehaviour
{
		// The text to say the time that has elapsed
		private Text elapsedTime;
		// Use this for initialization
		void Start()
		{
				//get the text component
				elapsedTime = GetComponent<Text>();

				//set the text to the time survived

				int minutes = (int)(GlobalControl.instance.timeSurvived / 60.0f);
				int seconds = (int)Mathf.Repeat(GlobalControl.instance.timeSurvived, 60.0f);
				int millis = (int)Mathf.Repeat(GlobalControl.instance.timeSurvived * 1000.0f, 1000.0f);
				elapsedTime.text = "Time elapsed : " + minutes.ToString().PadLeft(2, '0') + ":" +
						seconds.ToString().PadLeft(2, '0') + "." + millis.ToString().PadLeft(3, '0');

				if (GlobalControl.instance.levelPlayed == GlobalControl.instance.savedStats.unlockedLevels)
				{
						GlobalControl.instance.savedStats.unlockedLevels++;
				}

				//save the highscore in survival mode

				//check if it's equal or lower than the lowest highscore (last index)
				if (GlobalControl.instance.timeSurvived <=
						GlobalControl.instance.savedStats.highScores[GlobalControl.instance.savedStats.highScores.Length - 1])
				{
						//if so then it's not going to be saved
						return;
				}

				//Check if the score is the same as a score already in the array
				if (System.Array.IndexOf(GlobalControl.instance.savedStats.highScores, GlobalControl.instance.timeSurvived) > -1)
				{
						//don't save the same scores twice in the highscore array
						return;
				}

				//set the lowest (last index) to the new score 
				GlobalControl.instance.savedStats.highScores[GlobalControl.instance.savedStats.highScores.Length - 1] = GlobalControl.instance.timeSurvived;

				//Sort the array from highest at index 0 to lowest at index lenght-1

				//(bubble sort)
				for (int i = 0; i < GlobalControl.instance.savedStats.highScores.Length - 1; i++)
				{
						for (int j = i + 1; j < GlobalControl.instance.savedStats.highScores.Length; j++)
						{
								// check if the first position (i) is lower than the next position (j)
								if (GlobalControl.instance.savedStats.highScores[i] < GlobalControl.instance.savedStats.highScores[j])
								{
										//if the element at the first position is lower, then swap it
										float temp = GlobalControl.instance.savedStats.highScores[j];
										GlobalControl.instance.savedStats.highScores[j] = GlobalControl.instance.savedStats.highScores[i];
										GlobalControl.instance.savedStats.highScores[i] = temp;
								}
						}
				}
				//save the data
				GlobalControl.instance.SaveData();
		}
}
