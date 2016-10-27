using UnityEngine;
using System.Collections;

public class InvisibleRespawn : MonoBehaviour {
  public GameObject Respawn;
  public bool do_respawn;
  public bool highlight_reticule;

	// Use this for initialization
	void Start () {
    do_respawn = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnDestroy() {
    if (do_respawn) {
      Instantiate(Respawn, transform.position, Quaternion.identity);
    }
  }
}
