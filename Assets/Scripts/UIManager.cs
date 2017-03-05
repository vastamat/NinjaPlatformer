using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/** Manages all the UI functions */
public class UIManager : MonoBehaviour
{
		// The slider game object which controls the game volume
		private Slider volumeSlider;

		void Start()
		{
				volumeSlider = GetComponentInChildren<Slider>();
				if (volumeSlider != null)
				{
						// Set the sliders value to the saved volume from previous games
						volumeSlider.value = GlobalControl.instance.savedStats.volume;
						// Set the slider to inactive as it should come up when the music button is clicked
						volumeSlider.gameObject.SetActive(false);
				}
		}

		public void OnVolumeChanged(float value)
		{
				// When the value on the slider is changed, save it in the volume global data
				GlobalControl.instance.savedStats.volume = value;

				// And change the listener volume to it
				AudioListener.volume = value;
		}

		public void OnVolumeButtonClicked()
		{
				// When the music button is clicked, change it to it's inverse state
				volumeSlider.gameObject.SetActive(!volumeSlider.gameObject.activeSelf);
		}

		/*
   * When the back button is clicked, load the main menus scene.
   */
		public void OnBackClicked()
		{
				SceneManager.LoadScene("MainMenu");
		}

		/*
   * When the exit button is clicked, save the game data and exit the game
   */
		public void OnExitClicked()
		{
				//save the data
				GlobalControl.instance.SaveData();

				//exit the game
				Application.Quit();
		}

		public void OnLevelSelector()
		{
				// Load the level selector screen
				SceneManager.LoadScene("LevelSelector");
		}

		public void OnCreditsClicked()
		{
				// Load the credits screen
				SceneManager.LoadScene("Credits");
		}

		public void OnHighscoreClicked()
		{
				// Load the highscore screen
				SceneManager.LoadScene("Highscore");
		}

		/* Level selectors */

		public void OnLevel1()
		{
				GlobalControl.instance.levelPlayed = 1;
				SceneManager.LoadScene("lvl_1");
		}

		public void OnLevel2()
		{
				//Load level 2 if it's unlocked
				if (GlobalControl.instance.savedStats.unlockedLevels >= 2)
				{
						GlobalControl.instance.levelPlayed = 2;
						SceneManager.LoadScene("Level2");
				}
		}

		public void OnLevel3()
		{
				//Load level 3 if it's unlocked
				if (GlobalControl.instance.savedStats.unlockedLevels >= 3)
				{
						GlobalControl.instance.levelPlayed = 3;
						SceneManager.LoadScene("Level3");
				}
		}

		public void OnLevel4()
		{
				//Load level 4 if it's unlocked
				if (GlobalControl.instance.savedStats.unlockedLevels >= 4)
				{
						GlobalControl.instance.levelPlayed = 4;
						SceneManager.LoadScene("Level4");
				}
		}

		public void OnLevel5()
		{
				//Load level 5 if it's unlocked
				if (GlobalControl.instance.savedStats.unlockedLevels >= 5)
				{
						GlobalControl.instance.levelPlayed = 5;
						SceneManager.LoadScene("Level5");
				}
		}

		public void OnLevel6()
		{
				//Load level 6 if it's unlocked
				if (GlobalControl.instance.savedStats.unlockedLevels >= 6)
				{
						GlobalControl.instance.levelPlayed = 6;
						SceneManager.LoadScene("Level6");
				}
		}

		public void OnLevel7()
		{
				//Load level 7 if it's unlocked
				if (GlobalControl.instance.savedStats.unlockedLevels >= 7)
				{
						GlobalControl.instance.levelPlayed = 7;
						SceneManager.LoadScene("Level7");
				}
		}
		//..etc
}
