using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {

	public Text current;
	public Text high;

	public static int score;

	public static int highscore;

	void Start()
	{
		score = 0;
		highscore = PlayerPrefs.GetInt ("highscore", highscore);

		high.text = "HighScore: \n" + highscore.ToString();
		current.text = "Score: \n"+ score.ToString ();
	}

	void Update()
	{
		current.text = "Score: \n" + score.ToString();

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
	}

	public static void Reset()
	{
		score = 0;
	}

	public static void ResetHighScore (){
		highscore = 0;
	}
}

