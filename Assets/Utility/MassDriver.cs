using UnityEngine;
using System.Collections;

public class MassDriver : MonoBehaviour {
  Vector3 projectile_vector;
  Vector3 explosion_location;
  LifeController[] life_controllers;

  public LifeController.Owner owner;
	ExplosionMaker explosion_maker;

  AIUtility ai_util;

  // Use this for initialization
  void Start () {
    ai_util = GetComponent<AIUtility>();
    life_controllers = GameObject.FindObjectsOfType<LifeController>();
	}
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    transform.Translate(projectile_vector);

    if (explosion_maker != null) {
      if (Vector3.Distance(transform.position, explosion_location) < 0.05f) {
        explosion_maker.MakeBoom(transform.position);
        Destroy(gameObject);
      }
		}
	}

  public void SetProjectileVector(Vector3 v) {
    projectile_vector = v;
  }

  public void SetExplosionLocation(Vector3 location) {
    explosion_location = location;
    explosion_maker = GetComponent<ExplosionMaker>();
    explosion_maker.owner = owner;
  }
		
  void OnTriggerEnter2D(Collider2D other) {

    if (other.CompareTag("shredder")) {
      Destroy(gameObject);
    } else if (other.GetComponents<GunnableObject>().Length > 0) {
      if (GetComponents<GunObject>().Length > 0) {
        other.GetComponent<ScoreAdder>().destructor = owner;
        Destroy(other.gameObject);
        Destroy(gameObject);
      }
    } else if (other.GetComponents<PlayerController>().Length > 0) {
      print("Player is D-E-D");
      other.GetComponent<PlayerController>().MakeExplodingAnimation();
      Destroy(other.gameObject);
      Destroy(gameObject);

      DecideWhichPlayerIsBai(other.GetComponent<PlayerController>().owner);
    } else if (other.GetComponents<GunAbsorber>().Length > 0) {
      if (GetComponents<GunObject>().Length != 0) {
        Destroy(gameObject);
      }
    }
  }

  /// <summary>
  /// When there are more than one player, use this method to decide which one is dead
  /// </summary>
  /// <param name="dead_player">Identifier of dead player</param>
  void DecideWhichPlayerIsBai(LifeController.Owner dead_player) {
    foreach (LifeController lc in life_controllers) {
      if (lc.owner == dead_player) {
        lc.PlayerIsDead();
        break;
      }
    }
  }
}
