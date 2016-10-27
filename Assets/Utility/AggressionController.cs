using UnityEngine;
using System.Collections;

public class AggressionController : MonoBehaviour {
  float aggression = 0f;

  /// <summary>
  /// Get scaled aggression
  /// </summary>
  public int Aggression {
    get { return (int) aggression; }
  }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  /// <summary>
  /// Increase or decrease aggression
  /// Aggression is scaled between 0 to 6 and automatically clamped to fall between these values
  /// </summary>
  /// <param name="x">Adjust amount for each unit killed</param>
  public void AdjustAggression(float x) {
    aggression = Mathf.Clamp(aggression + x, 0f, 6.8f);
    print("Aggression = " + aggression + " (" + x + ")");
  }

}
