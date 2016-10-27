using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  AIUtility[] ai_utils;

  //public AudioClip backgroundMusic; 


  enum PlayerState { spawning, temporary_invincibility, in_game, undefined };
  PlayerState player_state = PlayerState.undefined;
  public KeyCode move_up;
  public KeyCode move_left;
  public KeyCode move_right;
  public KeyCode move_down;
  public KeyCode fire_gun;
  public KeyCode fire_bomb;
  public LifeController.Owner owner;

  public float gatling_reload_time;
  public float gatling_projectile_speed;
  public float bomb_reload_time;
  public float bomb_projectile_speed;
  public float move_speed_scaler;

  public GameObject on_land_reticule_prefab;
  ReticuleHighlighter the_reticule;

  public GameObject explosion_prefab;

  float gatling_timer;
  float bomb_timer;
  float invincibility_timer;

  Vector3 mandatory_translation;
  public AudioClip gatling_audio_clip;
  public AudioClip bomb_audio_clip;
  public AudioClip life_picked_up_clip;
  AudioSource my_audio_source;

  // Use this for initialization
  void Start() {
    ai_utils = GetComponentsInChildren<AIUtility>();
    the_reticule = GetComponentInChildren<ReticuleHighlighter>(); //GameObject.Find("Reticule");

    AssignShooter();
    my_audio_source = GetComponent<AudioSource>();
  }

  void AssignShooter() {
    foreach (AIUtility ai_util in ai_utils) {
      ai_util.owner = owner;
    }
  }

  // Update is called once per frame
  void Update() {
    if (ai_utils[0].is_paused()) { return; }

    switch (player_state) {
      case PlayerState.in_game:
        HandleMovement();
        HandleGun();
        HandleBomb();
        break;
      case PlayerState.temporary_invincibility:
        CountDownInvincibility();
        HandleMovement();
        HandleGun();
        HandleBomb();
        break;
      case PlayerState.spawning:
        GetInGame();
        break;
    }

  }

  void CountDownInvincibility() {
    invincibility_timer -= Time.deltaTime;
    if (invincibility_timer < 0) {
      player_state = PlayerState.in_game;
      GetComponent<BoxCollider2D>().enabled = true;
      GetComponent<SpriteBlinker>().enabled = false;
    }
  }

  void GetInGame() {
    if (transform.position.y > -7.0f) {
      player_state = PlayerState.temporary_invincibility;
      //GetComponent<BoxCollider2D>().enabled = true;
      invincibility_timer = 2.0f;
      //GetComponent<AudioSource>();
    }

    transform.Translate(mandatory_translation);
    mandatory_translation -= new Vector3(0, 0.001f, 0);
  }

  void FireGatling() {
    foreach (AIUtility gatling in ai_utils) {
      if (gatling.CompareTag("gatling")) {
        gatling.LaunchPlayerProjectile(gatling.transform.position, gatling_projectile_speed);
      }
    }

    my_audio_source.PlayOneShot(gatling_audio_clip);
  }

  void FireBomb() {
    foreach (AIUtility bomber in ai_utils) {
      if (bomber.CompareTag("bomb")) {
        //print (the_reticule.transform.position + " <->" + transform.position);
        float lifetime = 0.85f;

        bomber.CreateOnLandReticule(on_land_reticule_prefab, the_reticule.transform.position, lifetime);
        bomber.LaunchPlayerBomb(transform.position, bomb_projectile_speed, lifetime);
      }
    }

    my_audio_source.PlayOneShot(bomb_audio_clip);
  }

  void HandleGun() {
    gatling_timer += Time.deltaTime;
    if (gatling_timer > gatling_reload_time && Input.GetKey(fire_gun)) {
      gatling_timer = 0;
      FireGatling();
    }
  }

  void HandleBomb() {
    bomb_timer += Time.deltaTime;
    if (bomb_timer > bomb_reload_time && Input.GetKey(fire_bomb)) {
      bomb_timer = 0;
      FireBomb();
    }
  }

  void HandleMovement() {
    Vector3 movement_vector = new Vector3(0, 0, 0);
    int keys_pressed = 0;

    if (Input.GetKey(move_up)) {
      if (!ai_utils[0].HitTopBoundary(transform.position)) {
        keys_pressed++;
        movement_vector += new Vector3(0, 1, 0);
      }
    }
    if (Input.GetKey(move_down)) {
      if (!ai_utils[0].HitBottomBoundary(transform.position)) {
        keys_pressed++;
        movement_vector += new Vector3(0, -1, 0);
      }
    }
    if (Input.GetKey(move_left)) {
      if (!ai_utils[0].HitLeftBoundary(transform.position)) {
        keys_pressed++;
        movement_vector += new Vector3(-1, 0, 0);
      }
    }
    if (Input.GetKey(move_right)) {
      if (!ai_utils[0].HitRightBoundary(transform.position)) {
        keys_pressed++;
        movement_vector += new Vector3(1, 0, 0);
      }
    }

    switch (keys_pressed) {
      case 1:
        transform.Translate(movement_vector * move_speed_scaler);
        break;
      case 2:
        movement_vector *= (1 / Mathf.Sqrt(2));
        transform.Translate(movement_vector * move_speed_scaler);
        break;
    }
  }

  public void Spawn() {
    player_state = PlayerState.spawning;
    GetComponent<BoxCollider2D>().enabled = false;
    GetComponent<SpriteBlinker>().enabled = true;
    mandatory_translation = new Vector3(0, 0.2f, 0);
  }

  public void MakeExplodingAnimation() {
    Instantiate(explosion_prefab, transform.position, Quaternion.identity);
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponents<LifePickUp>().Length > 0) {
      my_audio_source.PlayOneShot(life_picked_up_clip);
    }
  }
}

