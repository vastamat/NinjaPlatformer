/*
	* Serializable class (for saving and loading) containing data to be saved when the games is turned off
	*/
[System.Serializable]
public class SavedData
{
		/*
			* the top 10 high scores
			*/
		public ushort[] highScores = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		/*
			* the unlocked levels (1 to num of levels)
			*/
		public byte unlockedLevels = 1;

		/*
			* music volume
			*/
		public float volume = 1.0f;

}