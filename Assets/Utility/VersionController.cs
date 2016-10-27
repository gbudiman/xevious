using UnityEngine;
using System.Collections;

public class VersionController : MonoBehaviour {
	public string version;
	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = "Build " + version;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
