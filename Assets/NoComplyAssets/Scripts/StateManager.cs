using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour {

	//Boolean to determine if the game is over
	private bool gameOver;
	private bool gameWin;
	private bool creditsShow;

	//Our player object
	private PlayerControl user;

	//Our Hud
	private UnityEngine.UI.Text hud;
	private UnityEngine.UI.Image healthBar;
	public UnityEngine.UI.Text gameOverText;

	//Our credits
	private Canvas credits;

	//Our score
	private int score;

	//Our slowmo
	private int currentSlowmo;
	private int maxSlowmo;
	private float slowmoRate;


	//Our background music
	public AudioSource bgFight;
	//Our background music
	public AudioSource deathSound;
	private bool deathPlayed;

	//Our select sound
	public AudioSource select;

	// Use this for initialization
	void Start () {

		//Scale our camera accordingly
		gameOver = false;
		gameWin = false;
		creditsShow = false;

		//Set our time to normal speed
		Time.timeScale = 1;

		//Get our player
		user = GameObject.Find ("Player").GetComponent<PlayerControl>();

		//Get our Hud
		//hud = GameObject.FindWithTag("Health Text").GetComponent<UnityEngine.UI.Text> ();
		//healthBar = GameObject.FindWithTag ("Health Bar").GetComponent<UnityEngine.UI.Image> ();

		//Get our Hud
		//credits = GameObject.FindGameObjectWithTag ("Credits").GetComponent<Canvas>();
		//credits.enabled = false;

		//get our bg music
		bgFight = GameObject.Find ("BG Song").GetComponent<AudioSource> ();
		//deathSound = GameObject.Find ("Death").GetComponent<AudioSource> ();
		deathPlayed = false;

		//Set score to zero
		score = 0;

		//Show our score and things
		//hud.text = ("Health: " + user.getHealth());

		//All of our slow mo stats
		maxSlowmo = 60;
		currentSlowmo = maxSlowmo;
		slowmoRate = 0.025f;

		//Hide Our gameover text
		//gameOverText.enabled = false;
	}

	// Update is called once per frame
	void Update () {

		//Check if we need to restart the game
		if(Input.GetAxis("Submit") != 0) {

			StartCoroutine ("resetScene");
		}

		//Check if we need to quite the Game
		if(Input.GetAxis("Cancel") != 0) Application.Quit();

		if (gameWin) {

			//Finalize our hud
			hud.text = ("Health: " + user.getHealth());
			healthBar.fillAmount = (user.getHealth () / 100.0f);

			//Slow down the game Time
			Time.timeScale = 0.275f;

			//Fade in the credits
			if (!creditsShow) {
				creditsShow = true;
				StartCoroutine ("creditsFade");
			}
		}
		else if(gameOver)
		{
			
			//Show our game over
			hud.text = "Health: 0";
			healthBar.fillAmount = 0.0f;

			//stop the music! if it is playing
			if(bgFight.isPlaying)
			{
				bgFight.Stop();
			}

			if (!deathPlayed) {
				//Play the Death Sounds
				//deathSound.Play();
				deathPlayed = true;
			}

			//Slow down the game Time
			Time.timeScale = 0.45f;

			//Show the Game Over Text
			gameOverText.enabled = true;
		}
		else {

			//Do normal stuff

			//Show our score and things
			//hud.text = ("Health: " + user.getHealth());
			//healthBar.fillAmount = (user.getHealth () / 100.0f);

			//start the music! if it is not playing
			if (!bgFight.isPlaying) {
				bgFight.Play ();
				bgFight.loop = true;
			}
				

			//Do some slowmo
			//Attacks with our player (Check for a level up here as well), only attack if not jumping
			if (Input.GetKeyDown (KeyCode.P) &&
				!gameOver && 
				Time.timeScale >= 1.0f) {

				//Now since we are allowing holding space to punch we gotta count for it
				if (currentSlowmo >= maxSlowmo) {

					//Time
					Time.timeScale = 0.25f;

					currentSlowmo = 0;
				}

			} else if (currentSlowmo < maxSlowmo) {

				currentSlowmo++;

				//Time
				if (Time.timeScale < 1.0f)
					Time.timeScale = Time.timeScale + slowmoRate;
				else
					Time.timeScale = 1.0f;
			}
		}

	}



	//Function to set gameover boolean
	public void setGameStatus(bool status)
	{
		gameOver = status;
	}

	//Function to get gameover boolean
	public bool getGameStatus()
	{
		return gameOver;
	}

	//Geter and setter for score
	public int getScore() {
		return score;
	}
	public void setScore(int newScore) {
		score = newScore;
	}

	//Function to reset the scene
	public IEnumerator resetScene() {

		//Play Selectect
		select.Play();

		//wait a tiny bit
		for(int i = 5; i > 0; i--) {
			yield return new WaitForFixedUpdate();
		}

		//Load the scene
		SceneManager.LoadScene ("GameMain");
	}

	//Function to fade in some credits
	public IEnumerator creditsFade() {

		//Pause and continue the music
		bgFight.Pause();

		//wait a tiny bit
		for(int i = 22; i > 0; i--) {
			yield return new WaitForFixedUpdate();
		}

		//Nestedloop for awesome ness
		for(int j = 17; j > 0; j--) {

			//Wait some frames
			for(int i = j / 3; i > 0; i--) {
				yield return new WaitForFixedUpdate();
			}

			//Disable the credits
			credits.enabled = false;
			bgFight.Pause ();

			//Wait some frames
			for(int i = j / 3; i > 0; i--) {
				yield return new WaitForFixedUpdate();
			}

			//Enable the credits
			credits.enabled = true;
			bgFight.Play ();
		}

		//Once More Super Fast Flashing
		for(int j = 3; j > 0; j--) {

			//Wait some frames
			yield return new WaitForFixedUpdate();

			//Disable the credits
			credits.enabled = false;
			bgFight.Pause ();

			//Wait some frames
			yield return new WaitForFixedUpdate();

			//Enable the credits
			credits.enabled = true;
			bgFight.Play ();
		}

	}
}