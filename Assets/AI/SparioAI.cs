using UnityEngine;
using System.Collections;

public class SparioAI : MonoBehaviour {
  enum AIState { aim, main }
  AIState ai_state;
  AIUtility ai_util;
  public float projectile_speed;
  public bool shoot_straight = false;
  public bool bypass_targeting = false;

  Vector3 movement_vector;

	// Use this for initialization
	void Start () {
    ai_util = GetComponent<AIUtility>();
    ai_state = AIState.aim;
	}
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    PerformStateTasks();
    HandleStateTransition();
	}

  void HandleStateTransition() {
    switch (ai_state) {
      case AIState.aim:
        ai_state = AIState.main;
        break;
    }
  }

  void PerformStateTasks() {
    switch (ai_state) {
      case AIState.aim:
        UpdateMovementVector();
        break;
      case AIState.main:
        MoveMyAss();
        break;
    }
  }

  void UpdateMovementVector() {
    if (bypass_targeting) { return; }

    if (shoot_straight) {
      // Torkan shoot straight
      float y_vector = ai_util.GetVectorToPlayer(transform.position).y;
      movement_vector = new Vector3(0, y_vector, 0).normalized;
    } else {
      movement_vector = ai_util.GetVectorToPlayer(transform.position).normalized;
      movement_vector = new Vector3(movement_vector.x, movement_vector.y, 0);
    }
  }

  /// <summary>
  /// Explicitly set targeting vector. Implicitly bypass targeting
  /// </summary>
  /// <param name="v">Targeting vector towards player</param>
  public void SetMovementVector(Vector3 v) {
    bypass_targeting = true;
    movement_vector = new Vector3(v.normalized.x, v.normalized.y, 0);
  }

  void MoveMyAss() {
    if (ai_util.is_paused()) { return; }
    transform.Translate(movement_vector * projectile_speed);
  }
}
