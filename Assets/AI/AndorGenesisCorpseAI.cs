using UnityEngine;
using System.Collections;

public class AndorGenesisCorpseAI : MonoBehaviour {
	Vector3 movement_vector;
	AIUtility ai_util;
	public float ease_factor;

	// Use this for initialization
	void Start () {
		ai_util = GetComponent<AIUtility>();
	}
	
	// Update is called once per frame
	void Update () {
		if (ai_util.is_paused()) { return; }
		UpdateMovementVector ();
		MoveMyAss ();
	}

	void UpdateMovementVector() {
		float y_vector = movement_vector.y - ease_factor;
		if (y_vector < -0.01f) {
			y_vector = -0.01f;
		}

		movement_vector = new Vector3 (0, y_vector, 0);

	}

	public void SetInitialVector(Vector3 init) {
		movement_vector = init;
	}

	void MoveMyAss() {
		transform.Translate(movement_vector);
	}
}
