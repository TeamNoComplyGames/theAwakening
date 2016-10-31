using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashSceneSwitch : MonoBehaviour {

	//The seconds untl the sceneswitches
	public int secondsUntilSceneSwitch = 3;
	private int frameCounter;

	//The Scene to switch to
	public string sceneName = "Scene Name From Build Settings";

	// Use this for initialization
	void Start () {
		secondsUntilSceneSwitch *= 60;
		frameCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(frameCounter >= secondsUntilSceneSwitch) SceneManager.LoadScene (sceneName);
		frameCounter++;
	}
}
