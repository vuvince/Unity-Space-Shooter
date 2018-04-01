using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {

	public Text current;
	public Text high;
	public Text bombCount;
	public Text level;
	public Text gameOver;

	public static int score;
	public static int bombs;
	public static bool over = false;

	public static int highscore;
	private static int pointsGained;
	public static int currLevel;


	void Start()
	{
		score = 0;
		pointsGained = 0;
		currLevel = 1;
		highscore = PlayerPrefs.GetInt ("highscore", highscore);

		high.text = "HighScore: \n" + highscore.ToString();
		current.text = "Score: \n"+ score.ToString ();
		bombCount.text = "Bombs: " + bombs.ToString ();
		level.text = "Level \n" + currLevel.ToString ();
		gameOver.text = "";
	}


	void Update()
	{
		current.text = "Score: \n" + score.ToString() + "\n+"+ pointsGained;
		bombCount.text = "Bombs: " + bombs.ToString ();
		level.text = "Level \n" + currLevel.ToString ();

		if (score > highscore)
		{
			highscore = score;
			high.text = "HighScore: \n" + highscore.ToString();
		}

		PlayerPrefs.SetInt ("highscore", highscore);

		if (over) {
			gameOver.text = "GameOver!\n\nScore: " + score.ToString ();
			over = false;
		}
	}



	public static void AddPoints (int pointsToAdd)
	{
		score += pointsToAdd;
		pointsGained = pointsToAdd;
	}

	public static void Reset()
	{
		score = 0;
	}

	public static int GetScore(){
		return score;
	}

	public static void ResetHighScore (){
		highscore = 0;
	}

	public static void UpdateBombs(int num)
	{
		bombs = num;
	}

	public static void LevelUp()
	{
		currLevel++;
	}

	public static void GameOver() {
		over = true;
	}


		
}

