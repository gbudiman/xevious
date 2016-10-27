using UnityEngine;
using System.Collections;

public class TemporaryInvincibility : MonoBehaviour {
  public float invincibility_period;
  float time_invincible;
  CircleCollider2D bob;

	// Use this for initialization
	void Start () {
    time_invincible = 0;
    bob = GetComponent<CircleCollider2D>();
    bob.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
    time_invincible += Time.deltaTime;
    if (time_invincible > invincibility_period) {
      bob.enabled = true;
    }
	}
}
