using UnityEngine;
using System.Collections;

public class GunnerAI : MonoBehaviour {
  AIUtility ai_util;
  AndorGenesisAI ai_ag;
  public float reload_time;
  public float jitter_between_fire;
  float time_since_last_shot;
  Animator animator;

	// Use this for initialization
	void Start () {
    ai_ag = GetComponentInParent<AndorGenesisAI>();
    ai_util = GetComponent<AIUtility>();
    ReRoll();
    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    Fire();
	}

  void Fire() {
    if (ai_ag != null && ai_ag.ai_state != AndorGenesisAI.AIState.move_out) { return;  }
    time_since_last_shot -= Time.deltaTime;

    if (animator && animator.GetFloat("AimDelay") > 0) {
      if (time_since_last_shot < animator.GetFloat("AimDelay")) {
        animator.SetBool("LogramIsAiming", true);
      }
    }

    if (time_since_last_shot < 0) {
      ai_util.LaunchProjectile(transform.position);
      if (animator) {
        animator.SetBool("LogramIsAiming", false);
      }
      ReRoll();
    }
  }

  void ReRoll() {
    float jitter = Random.Range(-jitter_between_fire, jitter_between_fire);
    time_since_last_shot = reload_time + jitter;
  }
}
