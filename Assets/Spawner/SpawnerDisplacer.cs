using UnityEngine;
using System.Collections;

public class SpawnerDisplacer : MonoBehaviour {
  public float max_y_boundary;
  public float min_y_boundary;
  public float interval_to_switch_direction;
  public float jitter;
  public float move_speed;

  float time_since_last_direction_change;
	// Use this for initialization
	void Start () {
    ReRollInterval();
	}
	
	// Update is called once per frame
	void Update () {
    MoveMyAss();
    HandleDirectionSwitch();
	}

  void HandleDirectionSwitch() {
    time_since_last_direction_change -= Time.deltaTime;

    if (time_since_last_direction_change < 0) {
      move_speed *= -1;
      ReRollInterval();
    }
  }

  void MoveMyAss() {
    float current_pos_y = transform.position.y;

    if (min_y_boundary < current_pos_y && current_pos_y < max_y_boundary) {

    } else {
      move_speed *= -1;
    }

    transform.Translate(0, move_speed, 0);
  }

    void ReRollInterval() {
    time_since_last_direction_change = interval_to_switch_direction + Random.Range(-jitter, +jitter);
  }
}
