using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {


	[Header("Set In Inspector")]
	public Vector2 rotMinMax = new Vector2 (15, 90);
	public Vector2 driftMinMax = new Vector2(.25f, 2);
	public float lifeTime = 6f;
	public float fadeTime = 4f;

	[Header("Set Dynamically")]
	public WeaponType type; //the type of the powerup
	public GameObject cube; //reference cube child
	public TextMesh letter; //reference textmesh
	public Vector3 rotPerSecond; //rot speed
	public float birthTime;


	private Rigidbody rigid;
	private BoundsCheck bndCheck;
	private Renderer cubeRend;


	// Use this for initialization
	void Awake () {
		//Find cube reference
		cube = transform.Find ("Cube").gameObject;
		//Find text mesh
		letter = GetComponent<TextMesh>();
		rigid = GetComponent<Rigidbody> ();
		bndCheck = GetComponent<BoundsCheck> ();
		cubeRend = cube.GetComponent<Renderer> ();

		//Set a random velocity
		Vector3 vel = Random.onUnitSphere;
		vel.z = 0;
		vel.Normalize ();
		vel *= Random.Range (driftMinMax.x, driftMinMax.y);
		rigid.velocity = vel;

		//Set rotation of this gameobject
		transform.rotation = Quaternion.identity;

		//Set up rotPerSecond
		rotPerSecond = new Vector3 (Random.Range (rotMinMax.x, rotMinMax.y),
			Random.Range (rotMinMax.x, rotMinMax.y),
			Random.Range (rotMinMax.x, rotMinMax.y));
		
		birthTime = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
		cube.transform.rotation = Quaternion.Euler (rotPerSecond * Time.time);
		//Given default values, powerup will exist for 10 sec and fade over 4 seconds

		float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

		if (u >= 1) {
			Destroy (this.gameObject);
			return;
		}

		if (u > 0) {
			Color c = cubeRend.material.color;
			c.a = 1f - u;
			cubeRend.material.color = c;
			//Fade the letter too, just not as much
			c = letter.color;
			c.a = 1f - (u * 0.5f);
			letter.color = c;
		}

		if (!bndCheck.isOnScreen) {
			Destroy (this.gameObject);
		}
	}

	public void SetType (WeaponType wt) {
		WeaponDefinition def = Main.GetWeaponDefinition (wt);

		cubeRend.material.color = def.color;

		letter.text = def.letter;
		type = wt;
	}


	//This function is called by Hero when powerup collected
	public void AbsorbedBy (GameObject target){
		Destroy (this.gameObject);
	}
}

