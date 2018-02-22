using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public float speed;
	public Camera boundary;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion angle;
		gameObject.transform.rotation = Quaternion.identity;
		Vector3 move = Vector3.zero;

		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		pos.x = Mathf.Clamp (pos.x, 0.05f, 0.95f);
		pos.y = Mathf.Clamp (pos.y, 0.05f, 0.95f);
		transform.position = Camera.main.ViewportToWorldPoint(pos);

		if (Input.GetKey (KeyCode.D)) {
			move += Vector3.right;
			angle = new Quaternion(0, 0, 10, -100);
			gameObject.transform.rotation = angle;
		}
		if (Input.GetKey (KeyCode.A)) {
			move += Vector3.left;
			angle = new Quaternion(0, 0, 10, 100);
			gameObject.transform.rotation = angle;
		}
		if (Input.GetKey (KeyCode.W)) {
			move += Vector3.up;
		}
		if (Input.GetKey (KeyCode.S)) {
			move += Vector3.down;
		}

		gameObject.transform.position = move * speed + gameObject.transform.position;

	}
}
