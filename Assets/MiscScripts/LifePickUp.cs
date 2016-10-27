using UnityEngine;
using System.Collections;

public class LifePickUp : MonoBehaviour {
  AudioSource my_audio_source;
	// Use this for initialization
	void Start () {
    my_audio_source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponents<PlayerController>().Length > 0) {
      print("CHECK!");
      LifeController.Owner owner = other.GetComponent<PlayerController>().owner;
      LifeController[] lcs = GameObject.FindObjectsOfType<LifeController>();

      foreach (LifeController lc in lcs) {
        if (lc.owner == owner) {
          lc.GrantExtraLife();
          break;
        }
      }
      Destroy(gameObject, 0.1f);
    }
  }
}
