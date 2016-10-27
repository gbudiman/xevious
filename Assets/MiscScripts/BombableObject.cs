using UnityEngine;
using System.Collections;

public class BombableObject : MonoBehaviour {
  public GameObject explosion_prefab;
  public GameObject crater_prefab;
	public bool has_dependent_sprite;

  InvisibleRespawn ivr;
	ScoreAdder sa;
	// Use this for initialization
	void Start () {
		sa = GetComponent<ScoreAdder> ();
    ivr = GetComponent<InvisibleRespawn>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponents<ExplosionPhysics>().Length > 0) {
			sa.is_destroyed_by_player = true;
      if (ivr != null) {
        ivr.do_respawn = true;
      }
    }
  }

  void OnDestroy() {
    if (sa.is_destroyed_by_player) {
      if (explosion_prefab != null) {
        Instantiate(explosion_prefab, transform.position, Quaternion.identity);
      }

      if (crater_prefab != null) {
        Instantiate(crater_prefab, transform.position, Quaternion.identity);
      }

			if (has_dependent_sprite) {
				//SpriteRenderer dependent = GetComponent<SpriteRenderer> ();
				DeadPoint.Position current_dp = GetComponent<DeadPoint>().position;
				ObjectIdentifier top_parent = GetComponentInParent<ObjectIdentifier> ();

				if (top_parent == null) {
					return;
				}

				DeadPoint[] dead_points = top_parent.GetComponentsInChildren<DeadPoint>();
				foreach (DeadPoint dead_point in dead_points) {
					if (dead_point.position == current_dp && !dead_point.is_parent) {
						dead_point.GetComponent<SpriteRenderer> ().enabled = true;
						break;
					}
				}
			}
    }
  }
}
