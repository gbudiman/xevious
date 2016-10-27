using UnityEngine;
using System.Collections;

public class MapSwitch : MonoBehaviour {

	public GameObject NextMap;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10.3) {
			SpawnNextMap();
		}
	
	}

	void SpawnNextMap(){
		var nextMapSpawner = GameObject.Find ("NextMapSpawner");
		if(nextMapSpawner){
			Vector3 pos = nextMapSpawner.transform.position;
			Instantiate (NextMap, pos, transform.rotation);
		}
		
	}
}
