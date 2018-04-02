using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	[Header("Set In Inpsector")]
	public float growFactor = 1f;

	private float maxSize = 40f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float timer = 0;

		timer += Time.deltaTime;
		transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * growFactor;

		if (gameObject.transform.localScale.y >= maxSize) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider go){
		GameObject otherGO = go.gameObject;
		//Projectile p = otherGO.GetComponent<Projectile> ();
		//bool notifiedOfDestruction = false;

		if (otherGO.tag == "Enemy") {
			Destroy (otherGO.gameObject);
		}

	}
}
