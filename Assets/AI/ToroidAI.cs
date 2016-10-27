using UnityEngine;
using System.Collections;

public class ToroidAI : MonoBehaviour {
  enum AIState { init, seek_player, seek_continue, fire, escaping };
  enum FiringState { wait, fire };
  public int fire_rate;
  public float reload_time;
  Animator animator;

  float time_since_last_fire;

  AIState ai_state;
  FiringState firing_state;
  AIUtility ai_util;
  float target_x;
  float target_y;
  public float x_move_scaler;
  public float y_move_constant;
  public float wait_intercept_min;
  public float wait_intercept_max;
  public float reverse_direction_x_scaler;

  float seek_direction;
  float escape_direction;
  float wait_to_reverse;
  float wait_threshold;
  float max_x_move_scaler;

	// Use this for initialization
	void Start () {
    ai_state = AIState.init;
    firing_state = FiringState.wait;
    max_x_move_scaler = 3.0f;
    ai_util = GetComponent<AIUtility>();
    seek_direction = 0;
    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    target_x = ai_util.GetPlayerPosition().x;
    target_y = ai_util.GetPlayerPosition().y;
    MoveUntilIntersectPlayerXLocation();
    Fire();
	}

  void MoveUntilIntersectPlayerXLocation() {
    switch(ai_state) {
      case AIState.init:
        // Initialize movement direction
        float diff_x = target_x - transform.position.x;
        seek_direction = diff_x > 0 ? 1 : -1;
        escape_direction = -2 * seek_direction;
        ai_state = AIState.seek_player;
        break;
      case AIState.seek_player:
        // Keep moving until encounter player's X position
        MoveMyAss();

        // Begin escape procedure only when player is in-front of Toroid
        if (target_y > transform.position.y) { return; }
        if (Mathf.Abs(transform.position.x - target_x) < 0.5) {
          ai_state = AIState.escaping;
          wait_to_reverse = 0;
          wait_threshold = Random.Range(wait_intercept_min, wait_intercept_max);
        }
        break;
      case AIState.escaping:
        ReverseDirectionAndAccelerate();
        PullHandbrake(y_move_constant / 2);
        animator.SetBool("StartToroidEscape", true);
        MoveMyAss();
        if (ai_util.IsOutsideViewport(transform.position, 1.0f)) {
          Destroy(gameObject);
        }
        break;
    }
  }

  void MoveMyAss() {
    transform.Translate(seek_direction * x_move_scaler, y_move_constant, 0);
  }

  void ReverseDirectionAndAccelerate() {
    if (Mathf.Abs(seek_direction) > max_x_move_scaler) { return; }

    if (escape_direction > 0) {
      // escape to right
      seek_direction += Time.deltaTime * reverse_direction_x_scaler;
    } else {
      // escape to left
      seek_direction -= Time.deltaTime * reverse_direction_x_scaler;
    }
  }

  void PullHandbrake(float target_factor) {
    if (y_move_constant > target_factor) {
      y_move_constant -= Time.deltaTime;
    }
  }

  void Fire() {
    if (ai_util.OnFiringLine(transform.position, ai_util.GetPlayerPosition(), 0.1f)) {
      firing_state = FiringState.fire;
      time_since_last_fire = 0;
    }

    if (firing_state == FiringState.fire) {
      time_since_last_fire += Time.deltaTime;

      if (time_since_last_fire > reload_time & fire_rate > 0) {
        time_since_last_fire = 0;
        ai_util.LaunchProjectile(transform.position);
        fire_rate--;
      }
    }
  }
}
