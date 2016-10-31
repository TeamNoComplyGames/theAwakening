using UnityEngine;
using System.Collections;

public class CoinCollect : MonoBehaviour {

	//Movement inherited from move right delete

	//Our sounds
	private AudioSource collectSound;

	//Our Rendered
	private SpriteRenderer spriteRenderer;

	//Our collider
	protected Collider2D coinCollider;

	//Get our StateManager
	StateManager stateManager;

	// Use this for initialization
	void Start () {
		stateManager = FindObjectOfType (typeof(StateManager)) as StateManager;
		coinCollider = GetComponent<Collider2D>();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		collectSound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Not Being Used, just need to find collisions with the player
	}

	//Function to check if we can jump again for collisions
	void OnCollisionEnter2D(Collision2D collision)
	{

		//Check if it is spikes
		if(collision.gameObject.tag == "Player" && !stateManager.getGameStatus()) {
			//Increase the score
			stateManager.setScore (stateManager.getScore() + 1);

			StartCoroutine ("Collect");
		}

		//Check if it is an enemy
		if (collision.gameObject.tag == "Enemy" ||
			collision.gameObject.tag == "EnemyDuck") {

			//Ignore the collision
			Physics2D.IgnoreCollision(collision.collider, coinCollider);
		}
	}

	//Function for Collecting
	IEnumerator Collect() {

		//Play the collect sound
		collectSound.Play();

		//Disabkle the spriteRenderer
		spriteRenderer.enabled = false;

		//Wait some frames
		for(int i = 0; i < 60; ++i){
			//Let the frame finish
			yield return new WaitForFixedUpdate();
		}

		//Delete the object
		Destroy(gameObject);
	}

}
