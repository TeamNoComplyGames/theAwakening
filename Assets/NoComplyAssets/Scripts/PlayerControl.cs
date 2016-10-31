using UnityEngine;
using System.Collections;

public class PlayerControl : BaseCharacter {

	//Our sounds
	private AudioSource jump;
	private AudioSource attack;
	private AudioSource attackHit;

	//Boolean to check if attacking
	bool attacking;

	//Boolean to check if we are sliding
	bool sliding;

	//Our Number of jumps we have done
	public float jumpHeight;
	public int maxJumps = 1;
	private int jumps;
	private bool jumpReady;
	private float acceleratedGravity;
	private bool grounded;
	public Transform groundCheck;
	public LayerMask groundLayer;
	private bool walled;
	public Transform wallCheck;
	public LayerMask wallLayer;
	public float groundCheckRadius;
	public float wallCheckRadius;


	//Counter for holding space to punch
	private int holdAttack;
	//How long do they have to hold before attacking
	public int holdDuration;

	//Amount of damage sword deals
	public int playerDamage;

	// Use this for initialization
	protected override void Start ()
	{
		//Call our superclass start
		base.Start();

		//Get our sounds
		jump = GameObject.Find ("Jump").GetComponent<AudioSource> ();
		/*
		attack = GameObject.Find ("Attack").GetComponent<AudioSource> ();
		attackHit = GameObject.Find ("AttackHit").GetComponent<AudioSource> ();
		*/
		//Set our actions
		attacking = false;
		sliding = false;
		jumps = 0;
		holdAttack = 0;

		//Set up our Jumping Ground Detection
		grounded = false;
		acceleratedGravity = 0.0f;
		jumpReady = true;

		//Get our collider
	}

	// Update is called once per frame
	protected override void Update ()
	{
		//Call our base update
		base.Update ();

		//check if dead, allow movement if alive
		if (curHealth > 0) {

			//Check ground/wall status
			//Following: Unity 2d Character Controllers
			grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
			walled = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

			//If walled, make Jumps = 1
			//if(walled) jumps = 1;
			//Recalculate our ground State
			if (grounded) {
				jumps = 0;
				acceleratedGravity = 0;
				charBody.drag = 0.5f;
			}

			if (!grounded) {
				
				charBody.AddForce (new Vector2 (0, acceleratedGravity * -7));
				acceleratedGravity = acceleratedGravity + 0.15f;
			}

			//Attacks with our player (Check for a level up here as well), only attack if not jumping
			/*
			if (Input.GetAxis("Attack") != 0 &&
				!gameManager.getGameStatus()) {

				//Now since we are allowing holding space to punch we gotta count for it
				if(!attacking && (holdAttack % holdDuration == 0))
				{
					//Set hold punch to zero
					holdAttack = 0;

					//Attacking working great
					StopCoroutine("Slash");
					StartCoroutine ("Slash");
				}

				//Increase hold punch
				holdAttack++;
			}

			//Check for attack Key Up
			if(Input.GetAxis("Attack") == 0) {

				//Set hold punch to zero
				holdAttack = 0;
			}
			*/

			//Jumping INput, cant jump if attacking
			if(Input.GetAxis("Jump") != 0 && !attacking && 
				jumpReady &&
				!sliding &&
				jumps < maxJumps &&
				!gameManager.getGameStatus()) {

				jumpReady = false;

				if (jump.isPlaying)
					jump.Stop ();
				jump.Play ();

				//Jump Coroutine
				StopCoroutine ("Jump");
				StartCoroutine ("Jump");
			}

			//Force to let go of axis
			if(Input.GetAxis("Jump") == 0) jumpReady = true;


			//Check if we can slide
			if(Input.GetAxis("Fire2") != 0 && 
				!attacking && 
				grounded && 
				jumps < maxJumps &&
				!sliding && 
				!gameManager.getGameStatus()) {
				//Slide Coroutine
				StopCoroutine ("Slide");
				animator.SetBool ("Sliding", false);
				StartCoroutine ("Slide");
			}
		} 
		else {

			//We is ded

			//make our player object invisible
			//possible display some animation first
			//Renderer r = (Renderer) gameObject.GetComponent("SpriteRenderer");
			//r.enabled = false;
			//No longer turning invisible, just looping death animation
			//play our death animation
			animator.SetTrigger ("Death");

			//play the death sound
			//if (!death.isPlaying) {
			//death.Play ();
			//}

			//Set our gameover text
			gameManager.setGameStatus (true);

			//set health to 0
			curHealth = 0;
		}
	}

	//Function for sliding
	IEnumerator Slide() {
		//Set shooting to true
		sliding = true;

		animator.SetBool ("Sliding", true);

		//Wait about 1 second (60 frames)
		for(int i = 0; i < 60; ++i) {
			//Let the frame finish
			yield return new WaitForFixedUpdate();
		}

		animator.SetBool ("Sliding", false);
		sliding = false;
	}

	//Function for attacking
	IEnumerator Slash() {

		//Set shooting to true
		attacking = true;

		animator.SetTrigger ("Attack");

		//Play the attack sound
		if(attack.isPlaying) attack.Stop();
		attack.Play ();

		//Knock Forward
		float knockMove = 0.0025f;
		//Get our directions
		if (direction == 1)
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x + knockMove, gameObject.transform.position.y, 0);
		else if (direction == -1)
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x - knockMove, gameObject.transform.position.y, 0);

		//Let the frame finish
		yield return new WaitForFixedUpdate();

		//set attacking to false
		attacking = false;
	}

	//Function for jumping
	IEnumerator Jump() {

		//Cancel the jump animation if jumping
		animator.SetBool ("Jump", false);
		yield return new WaitForFixedUpdate();

		//Increase our jumps
		jumps++;

		//Reset our gravity acceleration
		acceleratedGravity = 0.0f;

		//Reset our y velocity
		//And Decrease our X
		charBody.velocity = new Vector2 (charBody.velocity.x, 0);

		//Increase our drag
		charBody.drag = 25000000.0f;

		//Start jump animation
		animator.SetBool ("Jump", true);

		//Check if we need the walljump boolean
		bool wallForce = false;
		int wallDirection = direction * -1;
		if (walled) wallForce = true;

		//Add the jump force
		//Needs to be intervals of 30, gives best accleration
		for(int i = 35; i > 0; i--) {

			//Check Here to make sure we didn't get smacked back down
			if (i < 33 && (grounded)) {
				StopCoroutine ("Jump");
			}

			if (i < 20 && (walled)) {
				StopCoroutine ("Jump");
			}

			float jumpY = 395.0f * i * jumpHeight * Time.deltaTime;

			//Walljumping force
			float jumpX = 0;
			if (wallForce) {

				//Create our jumpX force
				jumpX = 345f * i * jumpHeight * Time.deltaTime * wallDirection;

				//Halve our jump force if we are facing the other way
				if (wallDirection != (direction * -1))
					jumpX = jumpX / 2;
			}

			//Add the Force
			charBody.AddForce( new Vector2(0, jumpY));

			//Force some camera Lerp
			actionCamera.addLerp(0, i / -1080.0f);


			//Wait some frames
			//Wait a frame
			yield return new WaitForFixedUpdate();
		}
	}

	//Function to check if we can jump again for collisions
	void OnCollisionEnter2D(Collision2D collision)
	{
		
		//Check if it is spikes
		if(collision.gameObject.tag == "SpikeWall") {
			//Kill the players
			setHealth(0);
		}

		//Check if it is the floor
		if (collision.gameObject.tag == "Floor" ||
			collision.gameObject.tag == "EnemyChar" ||
			collision.gameObject.tag == "BossChar") {
			
			//Set Jumps to zero
			StopCoroutine ("Jump");
			animator.SetBool ("Jump", false);
			acceleratedGravity = 0;
			jumps = 0;

			//Reset our drag
			charBody.drag = 0.5f;
			actionCamera.impactPause();
			actionCamera.startShake ();
		}

		//Check if it is an enemy
		if (collision.gameObject.tag == "Enemy" ||
			collision.gameObject.tag == "EnemyDuck") {

			//Ignore Collisions if dead
			if(gameManager.getGameStatus()) {
				Physics2D.IgnoreCollision(collision.collider, charCollider);
				return;
			}

			//First check if we were ducking, and it is a enemy we need to duck for
			if(sliding && collision.gameObject.tag == "EnemyDuck") {
				Physics2D.IgnoreCollision(collision.collider, charCollider);
				return;
			}

			if (attacking) {

				//Attack the enemy
				attackEnemy (collision);

				return;
			}
		}
	}

	//Function to check if we can jump again for collisions
	void OnCollisionStay2D(Collision2D collision)
	{

		//Check if it is spikes
		if (collision.gameObject.tag == "SpikeWall") {
			//Kill the players
			setHealth (0);
		}

		//Check if it is the floor
		if (collision.gameObject.tag == "Floor" ||
			collision.gameObject.tag == "EnemyChar" ||
			collision.gameObject.tag == "BossChar") {

			//Set Jumps to zero
			acceleratedGravity = 0;
			jumps = 0;
			//Reset our drag
			charBody.drag = 0.5f;
		}

		//Check if it is an enemy
		if (collision.gameObject.tag == "Enemy" ||
			collision.gameObject.tag == "EnemyDuck") {

			//Ignore Collisions if dead
			if(gameManager.getGameStatus()) {
				Physics2D.IgnoreCollision(collision.collider, charCollider);
				return;
			}

			//First check if we were ducking, and it is a enemy we need to duck for
			if(sliding && collision.gameObject.tag == "EnemyDuck") {
				Physics2D.IgnoreCollision(collision.collider, charCollider);
				return;
			}

			if (attacking) {

				//Attack the enemy
				attackEnemy (collision);
				return;
			}

			//Else, subtract one from our health
			setHealth(getHealth() - 1);
		}
	}

	public void attackEnemy(Collision2D collision) {

		//Do some damage
		//Check if the enemy is in the direction we are facing
		//Get our x and y
		float playerX = gameObject.transform.position.x;
		float playerY = gameObject.transform.position.y;
		float enemyX = collision.gameObject.transform.position.x;
		float enemyY = collision.gameObject.transform.position.y;

		//Our window for our Attack range (Fixes standing still no attack bug)
		float window = .05f;

		//Deal damage if we are facing the right direction
		if((direction == 1 && (enemyX + window) >= playerX) ||
			(direction == -1 && (enemyX - window) <= playerX))
		{

			//Shake the screen
			actionCamera.startShake();

			//Add slight impact pause
			actionCamera.startImpact();

			//Play the attack sound
			if(attackHit.isPlaying) attackHit.Stop();
			attackHit.Play ();
		}
	}
}