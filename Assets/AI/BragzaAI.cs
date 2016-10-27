using UnityEngine;

using System.Collections;



public class BragzaAI : MonoBehaviour {

  Vector3 movement_vector;

  public float movement_scaler;

  AIUtility ai_util;

  // Use this for initialization

  void Start () {

    ai_util = GetComponent<AIUtility>();

    movement_vector = new Vector3(0, 0, 0);

	}

	

	// Update is called once per frame

	void Update () {

    if (ai_util.is_paused()) { return; }

    movement_vector += new Vector3(0, Time.deltaTime * movement_scaler, 0);

    MoveMyAss();

	}



  void MoveMyAss() {

    transform.Translate(movement_vector);

  }

}

