using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private BoundsCheck bndCheck;
	public float speedDown = 0.15f;
	public float health = 2;
	public int score = 100;
	public float showDamageDuration = 0.1f;

	[Header("Set Dynamically: Enemy")]
	public Color[] originalColors;
	public Material[] materials;
	public bool showingDamage = false;
	public float damageDoneTime;
	public bool notifiedOfDestruction = false;



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


		if (otherGO.tag == "ProjectileHero") {
			ShowDamage ();
			Destroy (otherGO);
	
			health -= Main.GetWeaponDefinition (p.type).damageOnHit;
				
			if (health <= 0) {
				Destroy (this.gameObject);
			}

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
		Vector3 down = Vector3.down;
		this.transform.position = speedDown * down + gameObject.transform.position;
	}
}