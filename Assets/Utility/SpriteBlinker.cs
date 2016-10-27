using UnityEngine;
using System.Collections;

public class SpriteBlinker : MonoBehaviour {
  SpriteRenderer child_sprite;
  public float state_switch_delay;
  float time_since_switch;

	// Use this for initialization
	void Start () {
    SpriteRenderer[] child_sprites = GetComponentsInChildren<SpriteRenderer>();
    foreach (SpriteRenderer _child_sprite in child_sprites) {
      if (_child_sprite.GetComponents<PlayerSprite>().Length > 0) {
        child_sprite = _child_sprite;
        break;
      }
    }

    time_since_switch = 0;
	}
	
	// Update is called once per frame
	void Update () {
    time_since_switch += Time.deltaTime;
    if (time_since_switch > state_switch_delay) {
      child_sprite.enabled = !child_sprite.enabled;
      time_since_switch = 0;
    }
  }

  void OnDisable() {
    child_sprite.enabled = true;
  }
}
