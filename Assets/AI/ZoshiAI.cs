using UnityEngine;
using System.Collections;

public class ZoshiAI : MonoBehaviour {
  public enum FiringMode { single, arc, full_circle };
  enum AIState { init, main };
  AIState ai_state;
  AIUtility ai_util;

  Vector3 movement_vector;
  public bool has_warp_ability;
  public float reload_time;
  public float movement_speed;
  public bool self_destruct_after_firing;

  float existence_threshold = 15.0f;
  float existence_elapsed;
  float warp_threshold;
  float time_since_last_warp;
  float time_since_last_fire;
  float unpredictable_move_threshold;
  float time_since_last_unpredictable_move;

  float warp_min = 1.0f;
  float warp_max = 3.0f;

  // Unpredictable move random threshold
  float um_min = 0.5f;
  float um_max = 3.0f;

  public FiringMode firing_mode;

  void Start() {
    ai_util = GetComponent<AIUtility>();
    ai_state = AIState.init;
    movement_vector = new Vector3(0, 0, 0);

    existence_elapsed = 0;
    ReRollUnpredictableMoveThreshold();
    ReRollWarpThreshold();
  }

	void Update() {
    if (ai_util.is_paused()) { return; }
    HandleUpdate();
    if ( has_warp_ability && ai_state == AIState.main ) { HandleWarpAbility(); }
    if ( !has_warp_ability && ai_state == AIState.main ) { MakeUnpredictableMove(); }
  }

  void MakeUnpredictableMove() {
    time_since_last_unpredictable_move += Time.deltaTime;

    if (time_since_last_unpredictable_move > unpredictable_move_threshold) {
      int rand = Random.Range(0, 7) * 45;
      movement_vector = Quaternion.Euler(0, 0, rand) * movement_vector;
      ReRollUnpredictableMoveThreshold();

    }
  }

  void HandleWarpAbility() {
    time_since_last_warp += Time.deltaTime;
    existence_elapsed += Time.deltaTime;

    if (time_since_last_warp > warp_threshold) {
      ReRollWarpThreshold();
      ExecuteWarp();
    }
  }

  void ReRollWarpThreshold() {
    time_since_last_warp = 0;
    warp_threshold = Random.Range(warp_min, warp_max);
  }

  void ReRollUnpredictableMoveThreshold() {
    time_since_last_unpredictable_move = 0;
    unpredictable_move_threshold = Random.Range(um_min, um_max);
  }

  void ExecuteWarp() {
    if (existence_elapsed < existence_threshold) {
      transform.position = ai_util.PickRandomPointOutside(ai_util.GetPlayerPosition(3.0f), 2.0f);
      CalculateMovementVector();
    }
  }

  void HandleUpdate() {
    switch (ai_state) {
      case AIState.init:
        CalculateMovementVector();
        MoveMyAss(true);
        if (!ai_util.IsOutsideViewport(transform.position, -1.0f)) {
          existence_elapsed = 0;
          //time_since_last_warp = 0;
          time_since_last_fire = 0;
          //time_since_last_unpredictable_move = 0;
          ReRollUnpredictableMoveThreshold();
          ReRollWarpThreshold();
          ai_state = AIState.main;
        }

        break;
      case AIState.main:
        MoveMyAss();
        Fire();
        break;
    }
  }

  void CalculateMovementVector() {
    Vector3 player_position = ai_util.GetPlayerPosition(3.0f);
    movement_vector = (player_position - transform.position);
    float magnitude_scaler = movement_speed / Vector3.Magnitude(movement_vector);

    movement_vector *= magnitude_scaler;
  }

  /// <summary>
  /// <param name="do_not_destroy">Set do_not_destroy to true keep object in existence</param>
  /// <para>
  /// Useful on newly-spawned objects because there is no guarantee
  /// they will be spawned inside viewport
  /// </para>
  /// </summary>
  void MoveMyAss(bool do_not_destroy = false) {
    transform.Translate(movement_vector);
    if (!do_not_destroy && ai_util.IsOutsideViewport(transform.position, 1.0f)) {
      Destroy(gameObject);
    }
  }

  void Fire() {
    time_since_last_fire += Time.deltaTime;
		GetComponent<AudioSource> ().PlayDelayed(0.3f);
    if (time_since_last_fire > reload_time) {
      switch (firing_mode) {
        case FiringMode.single:
          ai_util.LaunchProjectile(transform.position);
          break;
        case FiringMode.arc:
          ai_util.LaunchProjectileArc(transform.position);
          break;
        case FiringMode.full_circle:
          ai_util.LaunchProjectileFullCircle(transform.position);
          break;

      }
      
      time_since_last_fire = 0;

      if (self_destruct_after_firing) {
        Destroy(gameObject);
      }
    }
  }
}
