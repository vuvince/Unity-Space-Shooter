using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	static public Main S;

	public GameObject[] enemies;
	public float spawnPerSecond = 0.5f;
	public float defaultPadding;



	public GameObject easter;

	// Use this for initialization
	void Start () {
		S = this;

		Invoke ("SpawnEnemy", 1f / spawnPerSecond);

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


	//Spawn enemy prefabs	
	public void SpawnEnemy(){
		//Getting a random #
		int rand = Random.Range(0, enemies.Length);
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

	
	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3 (0, 0, 0);
		if (Input.GetKey(KeyCode.Alpha6) && Input.GetKeyDown (KeyCode.Alpha9)) {
			Instantiate (easter, pos, Quaternion.identity);
		}
	}

	public void DelayedRestart (float delay)
	{
		Invoke ("Restart", delay); 
	}

	public void Restart(){
		//Reload the original scene
		SceneManager.LoadScene("Main");
	}


}
