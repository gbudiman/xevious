using UnityEngine;
using System.Collections;
public class GunAbsorber : MonoBehaviour {
  LifeController[] life_controllers;

  // Use this for initialization
  void Start () {
    life_controllers = GameObject.FindObjectsOfType<LifeController>();
  }
	
	// Update is called once per frame
	void Update () {
    
  }
  void OnTriggerEnter2D(Collider2D other) {
		
    if (other.GetComponents<PlayerController>().Length > 0) {
      other.GetComponent<PlayerController>().MakeExplodingAnimation();
      DecideWhichPlayerIsBai(other.GetComponent<PlayerController>().owner);
      Destroy(other.gameObject);
    }

    if (other.GetComponents<GunObject>().Length > 0) {
      GetComponent<AudioSource>().Play();
    }
  }

  void DecideWhichPlayerIsBai(LifeController.Owner dead_player) {
    foreach (LifeController lc in life_controllers) {
      if (lc.owner == dead_player) {
        lc.PlayerIsDead();
        break;
      }
    }
  }
}
