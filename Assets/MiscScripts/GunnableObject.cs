using UnityEngine;
using System.Collections;

public class GunnableObject : MonoBehaviour {
	ScoreAdder sa;
  LifeController[] life_controllers;
  public GameObject explosion_prefab;

	// Use this for initialization
	void Start () {
		sa = GetComponent<ScoreAdder> ();
    life_controllers = GameObject.FindObjectsOfType<LifeController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponents<GunObject>().Length > 0) {
      sa.is_destroyed_by_player = true;
    }

    if (other.GetComponents<PlayerController>().Length > 0) {
      other.GetComponent<PlayerController>().MakeExplodingAnimation();
      DecideWhichPlayerIsBai(other.GetComponent<PlayerController>().owner);
      
      Destroy(gameObject);
      Destroy(other.gameObject);
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

  void OnDestroy() {
    if (sa.is_destroyed_by_player) {
      if (explosion_prefab) {
        Instantiate(explosion_prefab, transform.position, Quaternion.identity);

      }
    }
  }
}
