using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public float speed;
	private BoundsCheck bound;

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
}
