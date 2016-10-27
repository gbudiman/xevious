using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
  public KeyCode escape_menu;
  public KeyCode continue_after_game_over;
  public GameObject player1_prefab;
  public GameObject player2_prefab;
  public float player_respawn_delay;

  float time_since_last_respawn;
  bool is_game_over = false;
  bool flag_check_gameover;
  List<LifeController.Owner> respawn_queue;
  MenuController menu_controller;

  bool invincibility = false;
  bool fast_travel = false;

  List<KeyCode> key_combo;
  float gameover_delay_check;
	// Use this for initialization
	void Start () {
    Screen.SetResolution(450, 800, false);
    SpawnPlayer(LifeController.Owner.p1);
    menu_controller = GameObject.FindObjectOfType<MenuController>();
    UpdateGameOver();
    flag_check_gameover = false;

    if (PlayerPrefs.GetInt("player_count") == 2) {
      SpawnPlayer(LifeController.Owner.p2);
    } else {
      DestroyUnnecessaryAdministrations();
    }

    respawn_queue = new List<LifeController.Owner>();
    key_combo = new List<KeyCode>();
    invincibility = false;
    fast_travel = false;
	}
  
  void DestroyUnnecessaryAdministrations() {
    ScoreController[] scs = GameObject.FindObjectsOfType<ScoreController>();
    LifeController[] lcs = GameObject.FindObjectsOfType<LifeController>();
    ScoreText[] sts = GameObject.FindObjectsOfType<ScoreText>();
    LifeIcon[] lis = GameObject.FindObjectsOfType<LifeIcon>();
    PlayerIndicator[] pis = GameController.FindObjectsOfType<PlayerIndicator>();

    foreach (ScoreController sc in scs) {
      if (sc.owner == LifeController.Owner.p2) {
        Destroy(sc.gameObject);
      }
    }

    foreach (LifeController lc in lcs) {
      if (lc.owner == LifeController.Owner.p2) {
        Destroy(lc.gameObject);
      }
    }

    foreach (ScoreText st in sts) {
      if (st.owner == LifeController.Owner.p2) {
        Destroy(st.gameObject);
      }
    }

    foreach (LifeIcon li in lis) {
      if (li.owner == LifeController.Owner.p2) {
        Destroy(li.gameObject);
      }
    }

    foreach (PlayerIndicator pi in pis) {
      if (pi.owner == LifeController.Owner.p2) {
        Destroy(pi.gameObject);
      }
    }
  }
	
	// Update is called once per frame
	void Update () {
    DecrementRespawnDelay();
    HandleKeyboardInput();
    DecrementGameOverCheckDelay();
	}

  void HandleKeyboardInput() {
    if (!is_game_over) {
      if (Input.GetKeyDown(escape_menu)) {
        menu_controller.Toggle();
      }
    } else {
      if (Input.GetKeyDown(continue_after_game_over)) {
        SceneManager.LoadScene(0);
      }
    }

    if (Input.GetKeyDown(KeyCode.LeftShift)) {
      key_combo.Clear();
			key_combo.Add (KeyCode.LeftShift);
  	}

    if (Input.GetKeyDown(KeyCode.I)) {
      key_combo.Add(KeyCode.I);
    }

    if (Input.GetKeyDown(KeyCode.K)) {
      key_combo.Add(KeyCode.K);
    }

    if (Input.GetKeyDown(KeyCode.M)) {
      key_combo.Add(KeyCode.M);
    }

		if (key_combo.Count == 2 && key_combo[0] == KeyCode.LeftShift && key_combo[1] == KeyCode.I) {
      invincibility = !invincibility;

      PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();

      foreach (PlayerController player in players) {
        SpriteBlinker sbl = player.GetComponent<SpriteBlinker>();
        BoxCollider2D bcl = player.GetComponent<BoxCollider2D>();

        sbl.enabled = invincibility;
        bcl.enabled = !invincibility;
      }
      key_combo.Clear();
    }
		if (key_combo.Count == 2 && key_combo[0] == KeyCode.LeftShift && key_combo[1] == KeyCode.K) {
      print("fast!!!");
      fast_travel = !fast_travel;

      GameObject.FindObjectOfType<MapScroller>().scroll_speed = fast_travel ? 0.05f : 0.01f;
      key_combo.Clear();
    }
		if (key_combo.Count == 2 && key_combo[0] == KeyCode.LeftShift && key_combo[1] == KeyCode.M) {
      print("fucking fast!!!");
      fast_travel = !fast_travel;

      GameObject.FindObjectOfType<MapScroller>().scroll_speed = fast_travel ? 0.5f : 0.01f;
      key_combo.Clear();
    }
  }

  void DecrementRespawnDelay() {
    if (respawn_queue.Count > 0) {
      time_since_last_respawn -= Time.deltaTime;

      if (time_since_last_respawn < 0) {
        LifeController.Owner player_to_respawn = respawn_queue[0];
        respawn_queue.RemoveAt(0);
        SpawnPlayer(player_to_respawn);
      }
    }
  }

  public void SpawnPlayerDelayed(LifeController.Owner owner) {
    time_since_last_respawn = player_respawn_delay;
    respawn_queue.Add(owner);
  }

  void SpawnPlayer(LifeController.Owner owner) {
    GameObject g_instance;
    if (owner == LifeController.Owner.p1) {
      g_instance = Instantiate(player1_prefab, new Vector3(-3f, -12, 0), Quaternion.identity) as GameObject;
    } else {
      g_instance = Instantiate(player2_prefab, new Vector3(+3f, -12, 0), Quaternion.identity) as GameObject;
    }

    g_instance.GetComponent<PlayerController>().Spawn();
  }

  public void CheckGameOver() {
    flag_check_gameover = true;
    gameover_delay_check = 0.5f;
  }

  void DecrementGameOverCheckDelay() {
    if (!flag_check_gameover) { return;  }

    gameover_delay_check -= Time.deltaTime;

    if (gameover_delay_check < 0) {
      CheckGameOverDelayed();
    }
  }

  void CheckGameOverDelayed() {
    int players_in_game = GameObject.FindObjectsOfType<PlayerController>().Length;
    int life_icons = GameObject.FindObjectsOfType<LifeIcon>().Length;

    if (players_in_game == 0 && life_icons == 0) {
      is_game_over = true;
      UpdateGameOver();
    }

    flag_check_gameover = false;
  }

  void UpdateGameOver() {
    GameObject.FindObjectOfType<GameOverText>().GetComponent<Text>().enabled = is_game_over;

    if (is_game_over) {
      GameObject.FindObjectOfType<HighScoreController>().UpdateHighScore();
    }
  }
}
