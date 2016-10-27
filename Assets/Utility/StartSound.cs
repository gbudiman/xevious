using UnityEngine;
using System.Collections;

public class StartSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Awake ();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<AudioSource> ().Play();

		if (Input.GetKeyDown(KeyCode.M)) {
			GetComponent<AudioSource> ().Play();
		}
	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
