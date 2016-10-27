using UnityEngine;
using System.Collections;

public class DeadPoint : MonoBehaviour {
	public enum Position { top_left, top_right, bottom_left, bottom_right };
	public Position position;
	public bool is_parent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableSprite() {
		GetComponent<SpriteRenderer> ().enabled = true;
	}
}
