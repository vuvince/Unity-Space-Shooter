using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour {

	public float camWidth, camHeight;
	public float radius = 1f;
	public bool keepOnScreen = true, isOnScreen;
	public bool offRight, offLeft, offDown, offUp;

	// Use this for initialization
	void Start () {
		camHeight = Camera.main.orthographicSize;
		camWidth = camHeight * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 pos = transform.position;

		offRight = offUp = offDown = offLeft = false;

		if (pos.x > camWidth - radius) {
			pos.x = camWidth - radius;
			isOnScreen = false;
			offRight = true;
		}
		if (pos.x < -camWidth + radius) {
			pos.x = -camWidth + radius;
			isOnScreen = false;
			offLeft = true;
		}
		if (pos.y > camHeight - radius) {
			pos.y = camHeight - radius;
			isOnScreen = false;
			offUp = true;
		}
		if (pos.y < -camHeight + radius) {
			pos.y = -camHeight + radius;
			isOnScreen = false;
			offDown = true;
		}

		if (keepOnScreen && !isOnScreen) {
			transform.position = pos;
			isOnScreen = true;
		}
	}

	void OnDrawGizmos () {
		//Vector3 cam = Camera.main.transform.position;
		if (!Application.isPlaying) return;
		Vector3 boundSize = new Vector3(camWidth* 2, camHeight* 2, 0.1f);
		Gizmos.DrawWireCube(Vector3.zero, boundSize);
	}
		

}
