using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
  bool is_displayed = false;
  public bool game_is_paused;
  public KeyCode mover_up;
  public KeyCode mover_down;
  public KeyCode confirm;
  ExitSelector[] exit_selectors;

  int exit_selector_index;
	// Use this for initialization
	void Start () {
    exit_selectors = GameObject.FindObjectsOfType<ExitSelector>();
    UpdateVisibility();
  }
	
	// Update is called once per frame
	void Update () {
    HandleKeyboardInput();
	}

  void HandleKeyboardInput() {
    if (!is_displayed) { return; }

    if (Input.GetKeyDown(mover_up) || Input.GetKeyDown(mover_down) ) {
      exit_selector_index++;
      exit_selector_index = exit_selector_index % 2;
      UpdateExitSelection();
    }

    if (Input.GetKeyDown(confirm)) {
      switch(exit_selector_index) {
        case 0: Toggle(); break;
        case 1: SceneManager.LoadScene(0); break;
      }
    }
  }

  void UpdateExitSelection() {
    foreach (ExitSelector exit_selector in exit_selectors) {
      exit_selector.GetComponent<Text>().enabled = (exit_selector.index == exit_selector_index);
    }
  }

  public void Toggle() {
    is_displayed = !is_displayed;
    game_is_paused = is_displayed;
    UpdateExitSelection();
    UpdateVisibility();
    exit_selector_index = 0;
    
    if (is_displayed) {
      UpdateExitSelection();
    }
  }

  void UpdateVisibility() {
    InGameMenu igm = GameObject.FindObjectOfType<InGameMenu>();
    Text[] igm_texts = igm.GetComponentsInChildren<Text>();

    foreach (Text igm_text in igm_texts) {
      igm_text.enabled = is_displayed;
    }

    foreach (ExitSelector exit_selector in exit_selectors) {
      exit_selector.GetComponent<Text>().enabled = is_displayed;
    }
  }
}
