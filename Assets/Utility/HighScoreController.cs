using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HighScoreController : MonoBehaviour {
  public bool override_high_score;
  int high_score;
  Text high_score_text;

	// Use this for initialization
	void Start () {
    high_score_text = GetComponent<Text>();
    GetOrInitializeHighScore();
    DisplayHighscore();
	}
	
	// Update is called once per frame
	void Update () {
	}

  void DisplayHighscore() {
    if (high_score > 0) {
      high_score_text.enabled = true;
      high_score_text.text = "highscore\n\n" + high_score.ToString();
    } else {
      high_score_text.enabled = false;
    }
  }

  void GetOrInitializeHighScore() {
    if (override_high_score) {
      high_score = 5000;
      return;
    }

    if (PlayerPrefs.HasKey("highscore")) {
      high_score = PlayerPrefs.GetInt("highscore");
    } else {
      PlayerPrefs.SetInt("highscore", 0);
    }
  }

  public void UpdateHighScore() {
    int existing_high_score = PlayerPrefs.GetInt("highscore");
    List<int> in_game_high_scores = new List<int>();

    foreach (ScoreController sc in GameObject.FindObjectsOfType<ScoreController>()) {
      in_game_high_scores.Add(sc.GetScore());
    }

    in_game_high_scores.Sort();
    in_game_high_scores.Reverse();
    if (in_game_high_scores[0] > existing_high_score) {
      PlayerPrefs.SetInt("highscore", in_game_high_scores[0]);
    }
  }
}
