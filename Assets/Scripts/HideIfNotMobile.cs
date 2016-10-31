using UnityEngine;
using System.Collections;

public class HideIfNotMobile : MonoBehaviour {

	// Use this for initialization
	void Start () {

		#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			gameObject.SetActive(false);
		#endif
	
	}
}
