using UnityEngine;
using System.Collections;

public class ChainReactor : MonoBehaviour {
  ScoreAdder sa;
  BombableObject[] remaining_children;

  void Start() {
    sa = GetComponent<ScoreAdder>();
  }

	void OnDestroy() { 
    if (sa.is_destroyed_by_player) {
      remaining_children = GetComponentsInChildren<BombableObject>();

      foreach (BombableObject remaining_child in remaining_children) {
        ScoreAdder csa = remaining_child.GetComponent<ScoreAdder>();
        csa.is_destroyed_by_player = true;
        csa.destructor = sa.destructor;
        Destroy(csa.gameObject);
      }
    }
  }
}
