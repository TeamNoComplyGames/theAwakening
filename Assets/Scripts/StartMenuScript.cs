using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour {

	//Our high score
	public UnityEngine.UI.Text highScoreText;

	//Our total coins text
	public UnityEngine.UI.Text totalCoinsText;

	//Our sounds
	private AudioSource startSound;

	//If we have started
	private bool started;

	// Use this for initialization
	void Start () {
		//Get the text from the inspector

		started = false;

		//Load our save
		SaveManager.loadSave();
		highScoreText.text = SaveManager.getSaveScore ().ToString();
		totalCoinsText.text = SaveManager.getCoins ().ToString();
		startSound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		//On input start the game
		if(!started && Input.GetAxis("Submit") != 0) {
			StartCoroutine ("StartGame");
		}
	
	}

	//Play the start sound, and load the scene
	IEnumerator StartGame() {

		started = true;
		startSound.Play ();

		for (int i = 0; i < 20; ++i) {
			yield return new WaitForFixedUpdate();
		}
		SceneManager.LoadScene ("MainGame");
	}
}
