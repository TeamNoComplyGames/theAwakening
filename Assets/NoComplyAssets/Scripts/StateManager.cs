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
	private UnityEngine.GameObject gameOverHud;

	//Our credits
	private Canvas credits;

	//Our score
	private int score;

	//Our background music
	public AudioSource bgFight;
	//Our background music
	public AudioSource deathSound;
	private bool deathPlayed;

	//Our select sound
	public AudioSource select;

	//How much the game difficulty will increase
	public int difficultyRate = 10;
	private int difficultyFrames;

	//If we saved our game or not
	private bool savedGame;

	// Use this for initialization
	void Start () {

		//Scale our camera accordingly
		gameOver = false;
		savedGame = false;

		//Set our time to normal speed
		Time.timeScale = 1;

		//Get our player
		user = GameObject.Find ("Player").GetComponent<PlayerControl>();

		//Get our Hud
		hud = GameObject.FindWithTag("ScoreHud").GetComponent<UnityEngine.UI.Text> ();
		gameOverHud = GameObject.FindWithTag("GameOverHud");

		//get our bg music
		bgFight = GameObject.Find ("BG Song").GetComponent<AudioSource> ();
		deathPlayed = false;

		//Set score to zero
		score = 0;

		//Show our score and things
		hud.text = ("Coins: " + score);

		//Get our difficulty rate
		difficultyFrames = 0;

		//Hide Our gameover text
		gameOverHud.SetActive(false);

		//Load our save game
		SaveManager.loadSave();
	}

	// Update is called once per frame
	void Update () {

		if(gameOver)
		{
			
			//Show our game over
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

			if (!savedGame) {
				
				//Show the Game Over Text, hide the coins text
				hud.enabled = false;
				gameOverHud.SetActive(true);

				//Save our collected Coins
				SaveManager.setCollectedCoins(SaveManager.getCoins() + score);

				//Edit the game over text with our information
				UnityEngine.UI.Text gameOverText = gameOverHud.GetComponent<UnityEngine.UI.Text>();
				gameOverText.text = "Game Over!";
				//Check if it was a high score
				if(score > SaveManager.getSaveScore()) {
					gameOverText.text += "\nNew High Score!";
					SaveManager.setHighScore (score);
				}
				gameOverText.text += "\nCollected Coins: " + score;
				gameOverText.text += "\nTotal Coins: " + SaveManager.getCoins();

				//Save the save
				SaveManager.saveSave();

				savedGame = true;
			}
		}
		else {

			//Do normal stuff

			//Show our score and things
			hud.text = ("Coins: " + score);

			//start the music! if it is not playing
			if (!bgFight.isPlaying) {
				bgFight.Play ();
				bgFight.loop = true;
			}

			//Check if we need to increase the game speed
			if (difficultyFrames >= (difficultyRate * 20)) {
				Time.timeScale = Time.timeScale + (difficultyRate / 1000.0f);
				difficultyFrames = 0;
			} else {
				difficultyFrames++;
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

	//Function to reset the level
	public void resetGame() {
		StartCoroutine ("resetScene");
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
		SceneManager.LoadScene ("MainGame");
	}

	//Function to quit the game
	public void quitGame() {
		Application.Quit();
	}
}