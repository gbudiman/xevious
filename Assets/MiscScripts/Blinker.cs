using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Blinker : MonoBehaviour {
  Text player_indicator;
  public float state_switch_delay;
  float time_since_switch;
	// Use this for initialization
	void Start () {
    player_indicator = GetComponent<Text>();
    time_since_switch = 0;
	}
	
	// Update is called once per frame
	void Update () {
    time_since_switch += Time.deltaTime;
    if (time_since_switch > state_switch_delay) {
      player_indicator.enabled = !player_indicator.enabled;
      time_since_switch = 0;
    }
	}
}
