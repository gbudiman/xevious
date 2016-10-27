using UnityEngine;
using System.Collections;

public class MapScrollerController : MonoBehaviour {



	public float scrollSpeed;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, scrollSpeed, 0);

		if (transform.position.y < -80.7) {
			Destroy (gameObject);
		}
	}
}
