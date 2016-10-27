using UnityEngine;
using System.Collections;

public class ReticuleHighlighter : MonoBehaviour {
  public Sprite reticule_default;
  public Sprite reticule_highlit;
  SpriteRenderer sprite_renderer;

	// Use this for initialization
	void Start () {
    sprite_renderer = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponents<InvisibleRespawn>().Length > 0) {
      if (other.GetComponent<InvisibleRespawn>().highlight_reticule) {
        sprite_renderer.sprite = reticule_highlit;
      }
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.GetComponents<InvisibleRespawn>().Length > 0) {
      if (other.GetComponent<InvisibleRespawn>().highlight_reticule) {
        sprite_renderer.sprite = reticule_default;
      }
    }
  }
}
