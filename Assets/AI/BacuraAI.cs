using UnityEngine;
using System.Collections;

public class BacuraAI : MonoBehaviour {
  AIUtility ai_util;
  float existence_time;

  public float movement_speed;
	// Use this for initialization
	void Start () {
    ai_util = GetComponent<AIUtility>();
    existence_time = 0;
  }
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    existence_time += Time.deltaTime;
    MoveMyAss();
	}

  void MoveMyAss() {
    transform.Translate(0, movement_speed, 0);

    if (existence_time > 1.0f && ai_util.IsOutsideViewport(transform.position, 1.0f)) {
      Destroy(gameObject);
    }
  }
}
