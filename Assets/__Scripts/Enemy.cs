using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	protected BoundsCheck bndCheck;

	[Header("Set In Inspector")]
	public float speedDown = 0.15f;
	public float fireRate = 0.3f; //Unused
	public float health = 2;
	public int score = 100;
	public float showDamageDuration = 0.1f;
	public float powerUpDropChance = 1f;

	[Header("Set Dynamically: Enemy")]
	public Color[] originalColors;
	public Material[] materials;
	public bool showingDamage = false;
	public float damageDoneTime;
	public bool notifiedOfDestruction = false;

	private GameObject lastTriggerGo;

	public Vector3 pos {
		get {
			return(this.transform.position);
		}
		set{
			this.transform.position = value;
		}
	}


	// Use this for initialization
	void Awake () {
		bndCheck = GetComponent<BoundsCheck> ();

		materials = Utils.GetAllMaterials (gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors [i] = materials [i].color;
		}
			
	}

	// Update is called once per frame
	void Update () {
		
		Move ();

		if (showingDamage && Time.time > damageDoneTime) {
			UnShowDamage ();
		}

		if (bndCheck != null && !bndCheck.isOnScreen)
		{
			if (bndCheck.offRight || bndCheck.offDown || bndCheck.offLeft) {
				Destroy (gameObject);
			}
		}
			
	}

	void OnTriggerEnter(Collider go){
		GameObject otherGO = go.gameObject;
		Projectile p = otherGO.GetComponent<Projectile> ();
		bool notifiedOfDestruction = false;

		if (otherGO == lastTriggerGo) {
			return;
		}
		lastTriggerGo = otherGO;

		if (otherGO.tag == "ProjectileHero") {
			ShowDamage ();
			Destroy (otherGO);

			if (bndCheck.offUp) {
				return;
			}
	
			health -= Main.GetWeaponDefinition (p.type).damageOnHit;
				
			if (health <= 0) {
				if (!notifiedOfDestruction) {

					notifiedOfDestruction = true;
					Main.S.ShipDestroyed (this);
				}


				Destroy (this.gameObject);
				ScoreManager.AddPoints (this.score);
			}

		}
		if (otherGO.tag == "Bomb") {
			
			if (bndCheck.offUp) {
				return;
			}
			
			if (!notifiedOfDestruction) {
				notifiedOfDestruction = true;
				Main.S.ShipDestroyed (this);

			}
			notifiedOfDestruction = true;

			Destroy (this.gameObject);
			ScoreManager.AddPoints (this.score);

		}
		
	}

	void ShowDamage() {
		foreach (Material m in materials) {
			m.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}

	void UnShowDamage() {
		for (int i = 0; i < materials.Length; i++) {
			materials [i].color = originalColors [i];
		}
		showingDamage = false;
	}

	protected virtual void Move() {
		Vector3 tempPos = pos;
		tempPos.y -= speedDown * Time.deltaTime;
		pos = tempPos;
	}
}