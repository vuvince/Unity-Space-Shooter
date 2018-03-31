using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

	static public Hero S; //Singleton

	[Header("Set In Inspector")]
	public float speed;
	public int bombAmmo = 1;
	public float projectileSpeed = 1f;
	public float fireRate = 0.2f;
	public float gameRestartDelay = 2f;
	public float powerUpTime = 10f;


	[Header("Set Dynamically")]
	public float shieldLevel = 1; //Set Dynamically
	public GameObject projectilePrefab;
	public GameObject bombPrefab;
	private BoundsCheck bound;
	public delegate void WeaponFireDelegate ();
	private bool invicibility = false;
	public float lastTime;
	public GameObject shield;


	public WeaponFireDelegate fireDelegate;


	private Renderer shieldRend;
	private GameObject lastTriggerGo = null;
	private Color shieldColor;






	void Awake () {
		ScoreManager.UpdateBombs (bombAmmo);
		if (S == null) {
			S = this;
		} else {
			Debug.LogError ("HeroAwake() - Attempted to Assign second Hero");
		}

		//GET THE SHIELD COMPONENT
		shield = transform.Find ("Shield").gameObject;

		shieldRend = shield.GetComponent<Renderer> ();
		shieldColor = shieldRend.material.color;


	//	fireDelegate += TempFire;
	}

	
	// Update is called once per frame
	void Update () {
		
		//DEALING WITH THE INVICIBILITY POWERUP
		if (invicibility) {
			if (Time.time - lastTime > powerUpTime) {
				invicibility = false;
				shieldRend.material.color = shieldColor;
			} 
		}


		gameObject.transform.rotation = Quaternion.identity;
		Vector3 move = Vector3.zero;

		/*
		if (bound.BoundaryCheck() != "in") {
			Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
			pos.x = Mathf.Clamp (pos.x, 0.05f, 0.95f);
			pos.y = Mathf.Clamp (pos.y, 0.05f, 0.95f);
			transform.position = Camera.main.ViewportToWorldPoint (pos);
		}
		*/
		Vector3 pos = gameObject.transform.position;

		//Firing bullet
		/*
		if (Input.GetKeyDown(KeyCode.Space) ){
			//Invoke ("TempFire", fireRate);
			TempFire();
		}
		*/

		//Shoot Weapon
		if (Input.GetKeyDown(KeyCode.Space) && fireDelegate != null) {
			fireDelegate ();
		}

		//Drop bomb
		if (Input.GetKeyDown (KeyCode.F) && bombAmmo >= 1) {
			bombAmmo--;
			ScoreManager.UpdateBombs (bombAmmo);
			DropBomb ();
		}


		if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			move += Vector3.right;
				transform.rotation = Quaternion.Euler(0, -30, -15);
		}
		if (Input.GetKey (KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow)) {
			move += Vector3.left;
				transform.rotation = Quaternion.Euler(0, 30, 15);
		}
		if (Input.GetKey (KeyCode.W)|| Input.GetKey(KeyCode.UpArrow)) {
			move += Vector3.up;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			move += Vector3.down;
		}

		if(Input.GetKey(KeyCode.Space)){
				gameObject.transform.Rotate(-30,0,0);
			}

		gameObject.transform.position = move * speed + gameObject.transform.position;

		//RESET THE HIGHSCORE
		if (Input.GetKeyDown (KeyCode.P)) {
			ScoreManager.ResetHighScore ();
		}

	}

	/*		//Shoot shit
	void TempFire(){
		GameObject bulletGO = Instantiate<GameObject> (projectilePrefab);
		bulletGO.transform.position = transform.position;
		Rigidbody rB = bulletGO.GetComponent<Rigidbody>();
	//	rB.velocity = Vector3.up * projectileSpeed;

		Projectile proj = bulletGO.GetComponent<Projectile> ();
		proj.type = WeaponType.blaster;
		float tSpeed = Main.GetWeaponDefinition (proj.type).velocity;
		rB.velocity = Vector3.up * tSpeed;

	}
	*/


	void OnTriggerEnter (Collider other)
	{
		Transform rootT = other.gameObject.transform.root;
		GameObject go = rootT.gameObject;

		if (go == lastTriggerGo) {
			return;
		}
		lastTriggerGo = go;

		if (go.tag == "Enemy") {
			if (invicibility) {
				Destroy (go);
			} else {
				
				//Destroy and restart game
				if (shieldLevel == 0) {
					Destroy (gameObject);
					ScoreManager.Reset ();
					Main.S.DelayedRestart (gameRestartDelay);

				} else {
					shieldLevel--;
					Destroy (go);
				}
			} 
		}
		else if (go.tag == "PowerUp") {
			AbsorbPowerUp (go);
			}
		}

	

	public void DropBomb(){
		GameObject bomb = Instantiate<GameObject> (bombPrefab);
		var spawn = S.transform.position;
		bomb.transform.position = new Vector3 (spawn.x, spawn.y, 2f);

	}

	public void AbsorbPowerUp (GameObject go){
		PowerUp pu = go.GetComponent<PowerUp> ();
		switch (pu.type) {
			//What will happen?
		case WeaponType.bomb:
			{
				if (bombAmmo >= 3) {
					ScoreManager.UpdateBombs (bombAmmo);
					break;
				}
				bombAmmo++;
				ScoreManager.UpdateBombs (bombAmmo);
				break;
			}
			//MAKE THE PLAYER INVINCIBLE FOR A COUPLE SECONDS
		case WeaponType.shield:
			{
				shieldRend.material.color = Color.cyan;
				shieldLevel = 4;
				invicibility = true;
				lastTime = Time.time;

				break;
			}
		
		}
		pu.AbsorbedBy (this.gameObject);
	}



}
