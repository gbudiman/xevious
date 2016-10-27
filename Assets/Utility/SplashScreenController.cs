using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour {
  public KeyCode mover_up;
  public KeyCode mover_down;
  public KeyCode selector;
  public AudioClip keypress_clip;
  AudioSource my_audio_source;

  int current_selected_player;
  PlayerSelector[] player_selectors;

	// Use this for initialization
	void Start () {
    Screen.SetResolution(450, 800, false);
    current_selected_player = 1;
    player_selectors = FindObjectsOfType<PlayerSelector>();
    EnableHighlighter();
    my_audio_source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
    HandleKeyboardInput();
	}

  void HandleKeyboardInput() {
    if (Input.GetKeyDown(mover_up) || Input.GetKeyDown(mover_down)) {
      current_selected_player++;
      if (current_selected_player > 2) {
        current_selected_player -= 2;
      }

      EnableHighlighter();
      my_audio_source.PlayOneShot(keypress_clip);
    }

    if (Input.GetKeyDown(selector)) {
      PlayerPrefs.SetInt("player_count", current_selected_player);
      SceneManager.LoadScene(1);
    }
  }

  void EnableHighlighter() {
    foreach (PlayerSelector player_selector in player_selectors) {
      player_selector.GetComponent<Text>().enabled = (player_selector.player == current_selected_player);
    }
  }
}
