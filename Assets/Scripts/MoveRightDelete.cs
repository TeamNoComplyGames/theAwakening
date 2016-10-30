using UnityEngine;
using System;
using System.Collections;

public class MoveRightDelete : MonoBehaviour {

	//Our Move Speed
	[Tooltip("How Much to add to our X position every frame eg 1 - 100")]
	public float moveRateX = 3.0f;
	private float xMoveTrue;
	[Tooltip("How Much to add to our Y position every frame eg 1 - 100")]
	public float moveRateY = 3.0f;
	private float yMoveTrue;

	//Our Delete Position
	[Tooltip("Our Max X position before deleting. Uses absolute value to decide the intended direction")]
	public int deleteX = 3;
	[Tooltip("Our Max X position before deleting. Uses absolute value to decide the intended direction")]
	public int deleteY = 3;

	// Use this for initialization
	void Start () {
		//Get our true values
		xMoveTrue = (float) moveRateX / 100.0f;
		yMoveTrue = (float) moveRateY / 100.0f;
	}
	
	// Update is called once per frame
	void Update () {

		//Check if we need to delete the object
		if(Math.Abs(gameObject.transform.position.x) > deleteX) Destroy(gameObject);
		if(Math.Abs(gameObject.transform.position.y) > deleteY) Destroy(gameObject);

		//Else, move the object
		Vector2 newPos = new Vector2(gameObject.transform.position.x + xMoveTrue, gameObject.transform.position.y + yMoveTrue);
		gameObject.transform.position = newPos;
	}
}
