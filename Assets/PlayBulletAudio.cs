using UnityEngine;
using System.Collections;

public class PlayBulletAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GetComponent<AudioSource> ().isPlaying) {
			GetComponent<AudioSource> ().Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
}
}
