using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {

	public Text current;
	public Text high;
	public Text bombCount;


	public static int score;
	public static int bombs;

	public static int highscore;
	private static int pointsGained;


	void Start()
	{
		score = 0;
		pointsGained = 0;
		highscore = PlayerPrefs.GetInt ("highscore", highscore);

		high.text = "HighScore: \n" + highscore.ToString();
		current.text = "Score: \n"+ score.ToString ();
		bombCount.text = "Bombs: " + bombs.ToString ();
	}


	void Update()
	{
		current.text = "Score: \n" + score.ToString() + "\n+"+ pointsGained;
		bombCount.text = "Bombs: " + bombs.ToString ();

		if (score > highscore)
		{
			highscore = score;
			high.text = "HighScore: \n" + highscore.ToString();
		}

		PlayerPrefs.SetInt ("highscore", highscore);


	}

	public static void AddPoints (int pointsToAdd)
	{
		score += pointsToAdd;
		pointsGained = 2*pointsToAdd;
	}

	public static void Reset()
	{
		score = 0;
	}

	public static void ResetHighScore (){
		highscore = 0;
	}

	public static void UpdateBombs(int num)
	{
		bombs = num;
	}


		
}

