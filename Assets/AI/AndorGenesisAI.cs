using UnityEngine;
using System.Collections;

public class AndorGenesisAI : MonoBehaviour {
  public enum AIState { move_in, fast_move, move_out }
  public AIState ai_state;
	public GameObject corpse;
	ScoreAdder sa;

  AIUtility ai_util;

  public float ease_speed;
  public float top_speed;

  float time_spent_in_fast_move;
  float time_spent_in_slow_move;
  Vector3 movement_vector;

	// Use this for initialization
	void Start () {
    ai_state = AIState.move_in;
    ai_util = GetComponent<AIUtility>();
    time_spent_in_fast_move = 1;
    time_spent_in_slow_move = 1;
		sa = GetComponent<ScoreAdder> ();
	}
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    HandleStateTransition();
    ExecuteStateTasks();
	}

  void HandleStateTransition() {
    switch (ai_state) {
      case AIState.move_in:
        if (!ai_util.IsOutsideViewport(transform.position, -0.5f)) {
          ai_state = AIState.fast_move;
        }
        break;
      case AIState.fast_move:
        if (transform.position.y > 0) {
          ai_state = AIState.move_out;
        }
        break;
      case AIState.move_out:
        break;
    }
  }

  void ExecuteStateTasks() {
    switch (ai_state) {
      case AIState.move_in:
        movement_vector = new Vector3(0, ease_speed, 0);
        break;
      case AIState.fast_move:
        time_spent_in_fast_move += Time.deltaTime;
        movement_vector += new Vector3(0, Time.deltaTime * 0.1f, 0);
        break;
			case AIState.move_out:
        time_spent_in_slow_move += Time.deltaTime;
        if (movement_vector.y > ease_speed) {
          movement_vector -= new Vector3(0, Time.deltaTime * 2, 0);
        }

        if (ai_util.IsOutsideViewport(transform.position, 2.5f)) { Destroy(gameObject);  }
        break;
    }

    MoveMyAss();
  }

	void OnDestroy() {
		if (sa.is_destroyed_by_player) {
			BombableObject[] dependents = GetComponentsInChildren<BombableObject> ();
			foreach (BombableObject dependent in dependents) {
				ScoreAdder csa = dependent.GetComponent<ScoreAdder> ();
        if (csa != null) {
          csa.is_destroyed_by_player = true;
          Destroy(dependent.gameObject);
        }
			}

      print("instantiating corpse");
			GameObject ins_corpse = Instantiate (corpse, transform.position, Quaternion.identity) as GameObject;
			ins_corpse.GetComponent<AndorGenesisCorpseAI> ().SetInitialVector (movement_vector);
		}
	}

  void MoveMyAss() {
    transform.Translate(movement_vector);
  }
}
