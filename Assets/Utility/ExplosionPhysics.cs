using UnityEngine;
using System.Collections;

public class ExplosionPhysics : MonoBehaviour {
  public float explosion_duration;
  public LifeController.Owner owner;

  AIUtility ai_util;
  float explosion_elapsed;

  void Start() {
    ai_util = GetComponent<AIUtility>();
    explosion_elapsed = 0;
  }

  void Update() {
    if (ai_util.is_paused()) { return; }
    explosion_elapsed += Time.deltaTime;
    if (explosion_elapsed > explosion_duration) {
      Destroy(gameObject);
    }
  }

  void OnTriggerStay2D(Collider2D other) {
    if (other.gameObject.GetComponents<BombableObject>().Length > 0) {
      other.GetComponent<ScoreAdder>().destructor = owner;
      Destroy(other.gameObject);
    }
  }
}
