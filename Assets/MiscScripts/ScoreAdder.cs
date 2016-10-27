using UnityEngine;
using System.Collections;

public class ScoreAdder : MonoBehaviour {
	public int score;
	public bool is_destroyed_by_player = false;
  public float aggression_increase;
  public LifeController.Owner destructor;
  AggressionController agc;

	ScoreController[] scs;

	void Start() {
    agc = GameObject.FindObjectOfType<AggressionController>();
	}

	void OnDestroy() {
		if (is_destroyed_by_player) {
      AddScoreToAppropriatePlayer(score);
      AddAggression();
			//sc.Add (score);
		}
	}

  void AddAggression() {
    agc.AdjustAggression(aggression_increase);
  }

  void AddScoreToAppropriatePlayer(int score) {
    scs = GameObject.FindObjectsOfType<ScoreController>();

    foreach (ScoreController sc in scs) {
      if (sc.owner == destructor) {
        sc.Add(score);
        break;
      }
    }
  }
}
