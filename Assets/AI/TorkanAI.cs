using UnityEngine;
using System.Collections;

public class TorkanAI : MonoBehaviour {
  enum AIState { init, hunt, fire, escape };
  AIState ai_state;
  AIUtility ai_util;

  public float projectile_speed;
  public int max_fire_count;
  public float move_speed_scaler;

  int fire_count;

  float escape_wait_threshold;
  float wait_threshold;
  float escape_vector_y;

  Vector3 shooting_position;

  void Start() {
    fire_count = 0;
    ai_util = GetComponent<AIUtility>();
    ai_state = AIState.init;
    escape_wait_threshold = Random.Range(0.1f, 0.5f);
    escape_vector_y = 0;
  }

  void Update() {
    if (ai_util.is_paused()) { return; }
    HandleUpdate();
  }

  void HandleUpdate() {
    switch (ai_state) {
      case AIState.init:
        shooting_position = ai_util.GetToShootingRangeInFrontOfPlayer(5.0f);

        if (ai_util.IsOutsideViewport(transform.position, -2.0f)) {
          ai_state = AIState.hunt;
        }
        break;
      case AIState.hunt:
        float step = move_speed_scaler * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, shooting_position, step);

        if (ai_util.WithinDistance(shooting_position, transform.position, 0.5f)) {
          ai_state = AIState.fire;
          wait_threshold = 0;
        }
        break;
      case AIState.fire:
        if (fire_count++ < max_fire_count) {
          ai_util.LaunchProjectile(transform.position, true);
        }

        wait_threshold += Time.deltaTime;

        if (wait_threshold > escape_wait_threshold) {
          ai_state = AIState.escape;
        }
        break;
      case AIState.escape:
        Ruuuuun();
        break;
    }
  }

  void Ruuuuun() {
    escape_vector_y += 4 * Time.deltaTime;
    transform.Translate(new Vector3(0, escape_vector_y, 0));
    if (ai_util.IsOutsideViewport(transform.position, 1.0f)) {
      Destroy(gameObject);
    }
  }
}
