using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy {

	[Header("Set In Inspector")]
	public float speed;
	public float directionChangePerScd = 0.5f;



	protected override void Move(){
		Vector3 move = Vector3.zero;

		//Left side
		if (num < 0.5) {
			move = Vector3.right;
		}
		//Right Side
		else {
			move = Vector3.left;
		}

		gameObject.transform.position = speed * move + gameObject.transform.position;


		base.Move ();
	}
}
