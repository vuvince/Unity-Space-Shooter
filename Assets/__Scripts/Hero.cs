using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

	static public Hero S; //Singleton

	public float speed;

	public float projectileSpeed = 1f;

	public float fireRate = 0.2f;

	private BoundsCheck bound;

	public float shieldLevel = 1; //Set Dynamically

	private GameObject lastTriggerGo = null;

	public float gameRestartDelay = 2f;

	public GameObject projectilePrefab;

	public delegate void WeaponFireDelegate ();

	public WeaponFireDelegate fireDelegate;

	void Awake () {
		if (S == null) {
			S = this;
		} else {
			Debug.LogError ("HeroAwake() - Attempted to Assign second Hero");
		}

	//	fireDelegate += TempFire;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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

		if (Input.GetKeyDown(KeyCode.Space) && fireDelegate != null) {
			fireDelegate ();
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
				gameObject.transform.Rotate(0,45,0);
			}

		gameObject.transform.position = move * speed + gameObject.transform.position;

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
			
			//Destroy and restart game
			if (shieldLevel == 0) {
				Destroy (gameObject);
				Main.S.DelayedRestart (gameRestartDelay);

			} else {
				shieldLevel--;
				Destroy (go);
			}
		}
	}



}
