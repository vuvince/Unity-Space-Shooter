using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private BoundsCheck bndCheck;
	public float speedDown = 0.15f;

	// Use this for initialization
	void Start () {
		bndCheck = GetComponent<BoundsCheck> ();
	}

	// Update is called once per frame
	void Update () {
		
		Move ();

		if (bndCheck != null && !bndCheck.isOnScreen)
		{
			if (bndCheck.offRight || bndCheck.offDown || bndCheck.offLeft) {
				Destroy (gameObject);
			}
		}
			
	}

	void OnTriggerEnter(Collider go){
		GameObject otherGO = go.gameObject;
		if (otherGO.tag == "ProjectileHero") {
			Destroy (otherGO);
			Destroy (gameObject);

		}
	}

	protected virtual void Move() {
		Vector3 down = Vector3.down;
		this.transform.position = speedDown * down + gameObject.transform.position;
	}
}