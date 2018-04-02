using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TO DO
//REFINE ENEMIES: DAMAGES, GIVE THEM SHIELDS, MAYBE A HEALTHBAR?
//REFINE SCORE: SOMETHING TO DO WITH THE CHILDREN??

//WEAPONS
public enum WeaponType {
	none,
	blaster,
	spread,
	bomb,
	shield,
	aimbot,
	speed

}

public class Main : MonoBehaviour {

	static public Main S;
	static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

	public GameObject[] enemies;
	public float spawnPerSecond = 0.5f;
	public float defaultPadding;
	public WeaponDefinition[] weaponDefinitions;


	public GameObject easter;

	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[] {
		WeaponType.blaster, WeaponType.spread, WeaponType.shield};

	[Header("Levelling Up")]
	public int level = 1;
	public int currScore = 0;
	public int pointsPerLevel = 1000;
	private Enemy lastEnemy;
	private int topSpeed = 10;


	// Use this for initialization
	void Awake () {
		S = this;

		Invoke ("SpawnEnemy", 1f / spawnPerSecond);

		WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			WEAP_DICT[def.type] = def;
		}

		// set the desired aspect ratio (the values in this example are
		// hard-coded for 16:9, but you could make them into public
		// variables instead so you can set them at design time)
		float targetaspect = 3f / 4f;

		// determine the game window's current aspect ratio
		float windowaspect = (float)Screen.width / (float)Screen.height;

		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;

		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();

		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{  
			Rect rect = camera.rect;

			rect.width = 1.0f;
			rect.height = scaleheight;
			rect.x = 0;
			rect.y = (1.0f - scaleheight) / 2.0f;

			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;

			Rect rect = camera.rect;

			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;

			camera.rect = rect;
		}
	}

	//ADD SOME CODE HERE TO MAKE COMPLETELY RANDOM SPAWNS

	//Spawn enemy prefabs	
	public void SpawnEnemy(){
		//Getting a random #
		int rand = Random.Range(0, enemies.Length-1); //DO NOT SPAWN THE LAST ENEMY
		GameObject spawn = Instantiate<GameObject> (enemies [rand]);

		//Position enemy ABOVE screen with random x position
		float enemyPadding = defaultPadding;
		if (spawn.GetComponent<BoundsCheck>() !=null) {
			enemyPadding = Mathf.Abs (spawn.GetComponent<BoundsCheck> ().radius);
		}

		//Set initial Position
		Vector3 pos = Vector3.zero;
		float xMin = -5.5f + enemyPadding;
		float xMax = 5.5f - enemyPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = 9;
		spawn.transform.position = pos;

		Invoke ("SpawnEnemy", 1f / spawnPerSecond);
	}


	//SPAWN NEW TYPE OF ENEMY AT LEVEL2 AND UP
	public void SpawnEnemyTwo(){
		//Getting a random #
		int rand = Random.Range(0, enemies.Length);
		GameObject spawn = Instantiate<GameObject> (enemies [rand]);
		spawn.GetComponent<Enemy>().speedDown = Random.Range(topSpeed - 4,topSpeed); //CHANGE THE SPEED ENEMIES NOW HAVE RANDOM SPEEDS
		if (spawn.GetComponent<Enemy> ().speedDown >= 10) {
			spawn.GetComponent<Enemy> ().score *= 2;
		}
		//Position enemy ABOVE screen with random x position
		float enemyPadding = defaultPadding;
		if (spawn.GetComponent<BoundsCheck>() !=null) {
			enemyPadding = Mathf.Abs (spawn.GetComponent<BoundsCheck> ().radius);
		}

		//Set initial Position
		Vector3 pos = Vector3.zero;
		float xMin = -5.5f + enemyPadding;
		float xMax = 5.5f - enemyPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = 9;
		spawn.transform.position = pos;

		Invoke ("SpawnEnemyTwo", 1f / (spawnPerSecond));
	}

	
	// Update is called once per frame
	void Update () {


		if (currScore >= pointsPerLevel) {
			LvlUp ();
			if (level == 2) {
				CancelInvoke ();
				spawnPerSecond *= 2;
				Invoke ("SpawnEnemyTwo", 1f / (spawnPerSecond));
			}
			//LEVEL 3 AND UP SPAWNS FASTER AND ENEMIES GET QUICKER
			if (level > 3) {
				spawnPerSecond *= 1.1f;
				topSpeed += 1;
			}

		}

		Vector3 pos = new Vector3 (0, 0, 0);
		if (Input.GetKey(KeyCode.Alpha6) && Input.GetKeyDown (KeyCode.Alpha9)) {
			pos.y = 9;
			Instantiate (easter, pos, Quaternion.identity);
		}
	}

	public void LvlUp() {
		level++;
		currScore -= pointsPerLevel;
		ScoreManager.LevelUp ();
	}

	public void DelayedRestart (float delay)
	{
		Invoke ("Restart", delay); 
	}

	public void Restart(){
		//Reload the original scene
		SceneManager.LoadScene("Main");
	}

	static public WeaponDefinition GetWeaponDefinition ( WeaponType wt) {
		if (WEAP_DICT.ContainsKey(wt) ){
			return (WEAP_DICT[wt]);
		}

			return (new WeaponDefinition());
	}


	//SPAWNS THE POWERUP
	public void ShipDestroyed (Enemy e) {
		
		if (e == lastEnemy) {
			return;
		}
		lastEnemy = e;

		currScore += e.score;

		if (Random.value <= e.powerUpDropChance) {
			//Choose which power up to pick
			int ndx = Random.Range(0,powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency [ndx];

			//Spawn a powerup
			GameObject go = Instantiate(prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp> ();

			pu.SetType (puType);

			//Set it to the position of the destroyed ship
			pu.transform.position = e.transform.position;
		}
	}



}

