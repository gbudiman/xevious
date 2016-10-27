using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {
	int score;
  Text pscore;

  public LifeController.Owner owner;
  public int fist_life_bonus_threshold = 600;
  public int iterative_bonus_threshold = 500;
  LifeController life_controller;

	// Use this for initialization
	void Start () {
		score = 0;
    //p1score = GameObject.FindGameObjectWithTag("p1score").GetComponent<Text>();
    

    AssignLifeText();
    AssignLifeController();

    Add(0);
  }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Add(int s) {
    CountBonus(s);
    score += s;
    pscore.text = score.ToString();
	}

  void CountBonus(int addition) {
    if (score < fist_life_bonus_threshold) {
      if ((score + addition) > fist_life_bonus_threshold) {
        life_controller.GrantExtraLife();
      }
    } else {
      int unfirst = score - fist_life_bonus_threshold;
      int unsecond = score + addition - fist_life_bonus_threshold;

      int f_mod = unfirst / iterative_bonus_threshold;
      int s_mod = unsecond / iterative_bonus_threshold;

      if (s_mod > f_mod) {
        life_controller.GrantExtraLife();
      }
    }
  }

  void AssignLifeController() {
    LifeController[] lcs = GameObject.FindObjectsOfType<LifeController>();
    foreach (LifeController lc in lcs) {
      if (lc.owner == owner) {
        life_controller = lc;
        break;
      }
    }
  }

  void AssignLifeText() {
    ScoreText[] sts = GameObject.FindObjectsOfType<ScoreText>();
    foreach (ScoreText st in sts) {
      if (st.owner == owner) {
        pscore = st.GetComponent<Text>();
      }
    }
  }

  public int GetScore() {
    return score;
  }
}
