using UnityEngine;
using System.Collections;

public class GrobdaAI : MonoBehaviour {
  enum AIState { move_into_play, main, avoidance_upwards, avoidance_downwards, delayed }
  AIState ai_state;
  AIUtility ai_util;

  public float move_speed_scaler;
  float move_speed;
  float turn_around_step;
  float turn_around_speed;
  float delay_threshold = 1f; // Delay between consecutive avoidance
  float time_since_avoidance;

	// Use this for initialization
	void Start () {
    ai_util = GetComponent<AIUtility>();
    ai_state = AIState.move_into_play;
    move_speed = -1 * move_speed_scaler;
    turn_around_speed = -move_speed;
    turn_around_step = Mathf.Abs(0.05f * move_speed);
	}
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    HandleStateTransition();
    HandleTasks();
	}

  void HandleTasks() {
    switch(ai_state) {
      case AIState.move_into_play:
        MoveMyAss();
        break;
      case AIState.main:
        MoveMyAss();
        break;
      case AIState.avoidance_upwards:
      case AIState.avoidance_downwards:
        Avoid();
        MoveMyAss();
        break;
      case AIState.delayed:
        MoveMyAss();
        break;
    }
  }

  void HandleStateTransition() {
    switch(ai_state) {
      case AIState.move_into_play:
        if (!ai_util.IsOutsideViewport(transform.position, 0.5f)) {
          ai_state = AIState.main;
        }
        break;
      case AIState.main:
        if (ai_util.IsBeingTargeted(transform.position, 1.0f)) {
          float probability_to_avoid = Random.value;

          if (probability_to_avoid > 0.5f) {
            if (turn_around_speed > move_speed) {
              // move speed is in negative Y direction
              // currently moving downwards, avoidance = move upwards
              ai_state = AIState.avoidance_upwards;
            } else {
              ai_state = AIState.avoidance_downwards;
            }
          }

        }
        break;
      case AIState.avoidance_downwards:
        if (move_speed < turn_around_speed) {
          ai_state = AIState.delayed;
          turn_around_speed *= -1;
          ReRollDelayDice();
        }
        break;
      case AIState.avoidance_upwards:
        if (move_speed > turn_around_speed) {
          ai_state = AIState.delayed;
          turn_around_speed *= -1;
          ReRollDelayDice();
        }
        break;
      case AIState.delayed:
        time_since_avoidance += Time.deltaTime;
        if (time_since_avoidance > delay_threshold) {
          turn_around_speed *= -1;
          move_speed *= -1;
          ai_state = AIState.main;
        }
        break;
    }
  }

  void MoveMyAss() {
    transform.Translate(new Vector3(0, move_speed, 0));
  }

  void Avoid() {
    switch (ai_state) {
      case AIState.avoidance_downwards:
        move_speed -= turn_around_step;
        break;
      case AIState.avoidance_upwards:
        move_speed += turn_around_step;
        break;
    }
  }

  void ReRollDelayDice() {
    time_since_avoidance = 0;
    delay_threshold = Random.Range(0.1f, 1.0f);
  }
}
