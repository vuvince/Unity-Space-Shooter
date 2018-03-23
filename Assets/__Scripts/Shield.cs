using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	public float rotationsPerSecond = 0.1f;

	public int levelShown = 0;

	Material mat;
	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {

		//Read current shield level
		int currLevel = Mathf.FloorToInt (Hero.S.shieldLevel);

		if (levelShown != currLevel) {
			levelShown = currLevel;
			//Adjust the texture and offset to display
			mat.mainTextureOffset = new Vector2 (0.2f*levelShown, 0);

		}
		//Rotate the shield visually
		float rZ = -(rotationsPerSecond*Time.time*360) % 360f;
		transform.rotation = Quaternion.Euler (0, 0, rZ);
	}
}
