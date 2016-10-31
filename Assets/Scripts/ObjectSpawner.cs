using UnityEngine;
using System;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

	//Array of Prefabs to be spawned
	[Tooltip("An Array of prefabs of objects")]
	public GameObject[] objects;

	//Our spawn rate
	[Tooltip("Our spawn rate per second 1 through e.g 300")]
	public int spawnRate = 5;
	private int trueSpawnRate = 0;
	private int currentSpawn = 0;

	// Use this for initialization
	void Start () {
		//Set our true spawn rate (Slightly longer than input)
		trueSpawnRate = spawnRate * 20;
		currentSpawn = trueSpawnRate;
	}
	
	// Update is called once per frame
	void Update () {

		if (currentSpawn <= 0 ) {
			//Spawn an enemy!
			if(objects.Length > 0)StartCoroutine("SpawnEnemy");
			currentSpawn = trueSpawnRate;
		} else currentSpawn--;
	}

	//Function to spawn and object
	IEnumerator SpawnEnemy() {

		Vector2 spawnPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);

		//Get a random number from the array
		int ranIndex = (int) Math.Floor(UnityEngine.Random.value * objects.Length);

		//Spawn our enemy
		Instantiate(objects[ranIndex], spawnPos, Quaternion.identity);

		yield return null;
	}
}
