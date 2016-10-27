using UnityEngine;
using System.Collections;

public class LifeController : MonoBehaviour {
  public enum Owner { p1, p2 };
  public Owner owner;
  AIUtility ai_util;
  public int starting_life_count;
  public int life_limit;
  public GameObject solvalou_life_prefab;
  Vector3 life_display_placeholder;
  int life_count;
  GameController game_controller;

	// Use this for initialization
	void Start () {
    game_controller = GameObject.FindObjectOfType<GameController>();
    ai_util = GetComponent<AIUtility>();
    life_count = starting_life_count;
    if (owner == LifeController.Owner.p1) {
      life_display_placeholder = ai_util.GetLeftCorner() + new Vector3(0.5f, 0.5f, 0);
    } else {
      life_display_placeholder = ai_util.GetRightCorner() + new Vector3(-0.5f, 0.5f, 0);
    }
    UpdateDisplay();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  /// <summary>
  /// Subtract one life
  /// </summary>
  public void PlayerIsDead() {
    life_count--;
    UpdateDisplay();

    if (life_count > 0) {
      game_controller.SpawnPlayerDelayed(owner);
    }

    game_controller.CheckGameOver();
  }

  /// <summary>
  /// Add one extra life
  /// </summary>
  public void GrantExtraLife() {
    if (life_count < life_limit) {
      life_count++;
      UpdateDisplay();
    }
  }

  void UpdateDisplay() {
    Vector3 current_position = life_display_placeholder;
    int processed_life = life_count;

    LifeIcon[] g = GameObject.FindObjectsOfType<LifeIcon>();

    foreach (LifeIcon l in g) {
      if (l.owner == owner) {
        Destroy(l.gameObject);
      }
    }

     
    while (processed_life-- > 0) {
      GameObject sv = Instantiate(solvalou_life_prefab, current_position, Quaternion.identity) as GameObject;
      sv.GetComponent<LifeIcon>().owner = owner;
      if (owner == LifeController.Owner.p1) {
        current_position += new Vector3(0.6f, 0, 0);
      } else {
        current_position -= new Vector3(0.6f, 0, 0);
      }
    }
  }
}
